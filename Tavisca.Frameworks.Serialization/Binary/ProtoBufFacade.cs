using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ProtoBuf;
using ProtoBuf.Meta;
using Tavisca.Frameworks.Serialization.Configuration;

namespace Tavisca.Frameworks.Serialization.Binary
{
    public sealed class ProtoBufFacade : ISerializationFacade, IDisposable
    {
        #region Fields

        private readonly IApplicationSerializationConfiguration _configuration;

        private readonly IDictionary<Type, SortedList<string, string>> _typeDic = new Dictionary<Type, SortedList<string, string>>();
        private readonly ISet<Type> _heirarchySet = new HashSet<Type>();
        private readonly IDictionary<Type, ICollection<Assembly>> _targetAssemblies = new Dictionary<Type, ICollection<Assembly>>();

        private static readonly string[] TypeExclusions = new[] { "Microsoft", "System", "mscorlib" };
        private static readonly ISet<Type> SerializerSet = new HashSet<Type>();
        private static readonly IDictionary<Type, RuntimeTypeModel> TypeModels = new Dictionary<Type, RuntimeTypeModel>();
        private static readonly ReaderWriterLockSlim Lock = new ReaderWriterLockSlim();
        private static readonly object MasterLock = new object();

#if DEBUG
        private static readonly List<Type> TypesFetched = new List<Type>();
#endif

        #endregion

        #region Constructors

        public ProtoBufFacade()
        {
            _configuration = SerializationConfigurationManager.GetConfiguration();
        }

        public ProtoBufFacade(IApplicationSerializationConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region Configuration

        public void ConfigureType<T>(bool recursive, bool configureChildren)
        {
            var targetType = typeof(T);

            ConfigureType<T>(targetType, recursive, configureChildren, new HashSet<Type>(),
                GetTypeModel(targetType), targetType);
        }

        private void ConfigureType<T>(Type type, bool recursive, bool configureChildren,
            ISet<Type> navigatedTypes, RuntimeTypeModel model, Type originalType)
        {
#if DEBUG
            TypesFetched.Add(type);
#endif

            if (IsValidType(type))
                model.Add(type, true);

            var baseTypes = GetRecursiveBase(type).ToArray();

            for (int i = 0; i < baseTypes.Length; i++)
            {
                var targetBaseType = baseTypes[i];

                if (IsValidType(targetBaseType))
                    model.Add(targetBaseType, true);

                if (IsValidType(targetBaseType) && targetBaseType.GetCustomAttribute<ProtoContractAttribute>() == null)
                    throw new InvalidOperationException("type does not have attribute: " + targetBaseType.AssemblyQualifiedName);

                var targetChild = i == 0 ? type : baseTypes[i - 1];

                ConfigureSubType(targetBaseType, targetChild, model, originalType);

                if (!recursive)
                    break;
            }

            if (configureChildren && IsValidType(type))
            {
                var children = GetReferencedTypes(type).Concat(GetChildren(type, originalType)).Distinct().ToList();

                if (children.Any(x => IsValidType(x) && x.GetCustomAttribute<ProtoContractAttribute>() == null))
                {
                    throw new InvalidOperationException("type does not have attribute: " +
                        children.Where(x => IsValidType(x) && x.GetCustomAttribute<ProtoContractAttribute>() == null).Select(x => x.AssemblyQualifiedName)
                        .Aggregate((s, s1) => s + Environment.NewLine + s1));
                }

                foreach (var child in children)
                {
                    if (!navigatedTypes.Add(child))
                        continue;

                    ConfigureType<NullType>(child, recursive, true, navigatedTypes, model, originalType);
                }
            }

            if (typeof(T) != typeof(NullType))
                PrepareSerializer<T>(model);
        }

        private RuntimeTypeModel GetTypeModel(Type type)
        {
            RuntimeTypeModel model;
            Lock.EnterReadLock();
            try
            {
                if (TypeModels.TryGetValue(type, out model))
                    return model;
            }
            finally
            {
                Lock.ExitReadLock();
            }

            model = TypeModel.Create();
            model.AutoAddMissingTypes = true;
            model.UseImplicitZeroDefaults = true;
            model.AutoCompile = true;

            Lock.EnterWriteLock();
            try
            {
                if (!TypeModels.ContainsKey(type))
                    TypeModels[type] = model;
            }
            finally
            {
                Lock.ExitWriteLock();
            }

            return model;
        }

        private class NullType
        { }

        private void ConfigureSubType(Type baseType, Type type, RuntimeTypeModel model, Type originalType)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (IsValidType(baseType) && _heirarchySet.Add(type))
            {
                model.Add(baseType, true).AddSubType(GetFieldNumber(baseType, GetChildren(baseType, originalType).ToArray(), type, originalType), type);
            }
        }

        private int GetFieldNumber(Type baseType, Type[] subTypes, Type subType, Type originalType)
        {
            SortedList<string, string> sortedList;

            var bases = GetRecursiveBase(subType).ToArray();

            var baseMost = subType;
            if (bases.Any())
                baseMost = bases.Last();

            if (_typeDic.ContainsKey(baseMost))
                sortedList = _typeDic[baseMost];
            else
            {
                sortedList = new SortedList<string, string>(subTypes.Length);

                foreach (var type in subTypes)
                {
                    sortedList.Add(type.FullName, type.FullName);
                }
                _typeDic[baseMost] = sortedList;
            }

            var number = CalculateFieldNumber(Math.Abs(GetHashCode32(baseMost.FullName)),
                sortedList.IndexOfValue(subType.FullName));

            return number;
        }

        private static int GetHashCode32(string value)
        {
            var chars = value.ToCharArray();
            var lastCharInd = chars.Length - 1;
            var num1 = 0x15051505;
            var num2 = num1;
            var ind = 0;
            while (ind <= lastCharInd)
            {
                var ch = chars[ind];
                var nextCh = ++ind > lastCharInd ? '\0' : chars[ind];
                num1 = (((num1 << 5) + num1) + (num1 >> 0x1b)) ^ (nextCh << 16 | ch);
                if (++ind > lastCharInd) break;
                ch = chars[ind];
                nextCh = ++ind > lastCharInd ? '\0' : chars[ind++];
                num2 = (((num2 << 5) + num2) + (num2 >> 0x1b)) ^ (nextCh << 16 | ch);
            }
            return num1 + num2 * 0x5d588b65;
        }

        private int CalculateFieldNumber(int baseNumber, int child)
        {
            return checked(Normalize(baseNumber) + (child * 4) + 4);
        }

        private int Normalize(int number)
        {
            const int nFactor = 23827;

            return Math.Abs(number % nFactor);
        }

        private bool IsValidType(Type type)
        {
            return type != null && type != typeof(object) && type != typeof(ValueType)
                   && type != typeof(Enum)
                   && type.Namespace != null
                   && type.IsArray == false
                   && type.Namespace.Contains("Tavisca") &&
                   type.GetCustomAttribute<ProtoIgnoreAttribute>() == null;
        }

        private void PrepareSerializer<T>(RuntimeTypeModel model)
        {
            if (!SerializerSet.Add(typeof(T)))
            {
                model.CompileInPlace();
                model.Compile();
            }
        }

        private IEnumerable<Type> GetReferencedTypes(Type type)
        {
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).AsEnumerable();

            var baseTypes = GetRecursiveBase(type);

            foreach (var baseType in baseTypes)
            {
                fields = fields.Concat(baseType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
            }

            return fields.Where(x => x.GetCustomAttribute<ProtoIgnoreAttribute>() == null).SelectMany(x => GetDetailedTypes(x.FieldType)).Distinct();
        }

        private IEnumerable<Type> GetDetailedTypes(Type type)
        {
            if (type.IsGenericType)
            {
                foreach (var genericTypeArgument in type.GenericTypeArguments)
                {
                    yield return genericTypeArgument;
                }
            }

            yield return type;
        }

        private IEnumerable<Type> GetChildren(Type baseType, Type originalType)
        {
            if (baseType == null)
                throw new ArgumentNullException("baseType");

            var targetAssemblies = GetTargetAssemblies(originalType);

            var targets = targetAssemblies.SelectMany(x =>
            {
                try
                {
                    return x.GetTypes();
                }
                catch (ReflectionTypeLoadException)
                {
                    return new Type[0];
                }
            });

            var list = new List<Type>();

            foreach (var type in targets)
            {
                if (baseType == type)
                    continue;

                if (baseType.IsAssignableFrom(type))
                {
                    var parameterlessConstructor = type.GetConstructor(Type.EmptyTypes);

                    if (parameterlessConstructor != null)
                    {
                        list.AddRange(GetChildren(type, originalType));
                        list.Add(type);
                    }
                }
            }
            list.AddRange(GetRecursiveBase(baseType));

            return list.Distinct();
        }

        private IEnumerable<Assembly> GetTargetAssemblies(Type originalType)
        {
            ICollection<Assembly> retVal;

            if (_targetAssemblies.TryGetValue(originalType, out retVal))
                return retVal;

            var config = _configuration;

            var assemblies = config.GetAssemblyNameByType(originalType).ToArray();

            var validAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => !TypeExclusions.Any(y => x.FullName.StartsWith(y)));

            if (assemblies.Any())
            {
                validAssemblies =
                    validAssemblies.Where(
                        x => assemblies.Any(
                            y => y.Name.Equals(x.FullName.Substring(0, x.FullName.IndexOf(',')),
                                StringComparison.InvariantCultureIgnoreCase)));
            }

            retVal = validAssemblies.ToList();

            _targetAssemblies[originalType] = retVal;

            return retVal;
        }

        private IEnumerable<Type> GetRecursiveBase(Type type)
        {
            var lType = type;

            while (IsValidType(lType.BaseType))
            {
                yield return lType.BaseType;

                lType = lType.BaseType;
            }
        }

        private Type GetTargetedType(Type type)
        {
            if (typeof(ICollection).IsAssignableFrom(type))
            {
                if (type.IsGenericType)
                    return type.GetGenericArguments().First();

                if (type.IsArray)
                    return type.GetElementType();
            }

            return type;
        }

        private RuntimeTypeModel EnsureConfigured(Type obj)
        {
            if (!SerializerSet.Contains(obj))
            {
                lock (MasterLock)
                {
                    if (!SerializerSet.Contains(obj))
                    {
                        var method = this.GetType().GetMethod("ConfigureType");
                        var genericMethod = method.MakeGenericMethod(obj);
                        genericMethod.Invoke(this, new object[] { true, true });
                    }
                }
            }

            return GetTypeModel(obj);
        }

        private RuntimeTypeModel EnsureConfigured<T>()
        {
            return EnsureConfigured(GetTargetedType(typeof(T)));
        }

        #endregion

        #region Serialization

        public byte[] Serialize(object obj)
        {
            if (obj == null)
                return null;

            var model = EnsureConfigured(GetTargetedType(obj.GetType()));

            using (var memStream = new MemoryStream())
            {
                model.Serialize(memStream, obj);

                var data = memStream.ToArray();

                return data;
            }
        }

        public T Deserialize<T>(byte[] data)
        {
            if (data == null)
                return default(T);

            var model = EnsureConfigured<T>();

            using (var memStream = new MemoryStream(data))
            {
                return (T)model.Deserialize(memStream, null, typeof(T));
            }
        }

        #endregion

        #region Utility

        public T DeepClone<T>(T obj)
        {
            var model = EnsureConfigured<T>();

            return (T)model.DeepClone(obj);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Lock.Dispose();
        }

        #endregion
    }
}
