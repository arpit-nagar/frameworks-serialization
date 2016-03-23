using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tavisca.Frameworks.Serialization.Binary
{
    public sealed class DataContractJsonSerializerFacade : ISerializationFacade
    {
        public byte[] Serialize(object obj)
        {
            if (obj == null)
                return null;

            var ser = GetJsonSerializer(obj.GetType());

            using (var ms = new MemoryStream())
            {
                ser.WriteObject(ms, obj);

                var data = ms.ToArray();

                return data;
            }
        }

        public T Deserialize<T>(byte[] data)
        {
            if (data == null)
                return default(T);

            var ser = GetJsonSerializer(typeof(T));

            using (var ms = new MemoryStream(data))
            {
                var retVal = ser.ReadObject(ms);

                return (T)retVal;
            }
        }

        public T DeepClone<T>(T obj)
        {
            var serialized = Serialize(obj);

            var copy = Deserialize<T>(serialized);

            return copy;
        }

        private static readonly Dictionary<Type, DataContractJsonSerializer> JsonSerializerCache = new Dictionary<Type, DataContractJsonSerializer>();
        private static readonly ReaderWriterLockSlim SyncJsonObject = new ReaderWriterLockSlim();

        private static DataContractJsonSerializer GetJsonSerializer(Type type)
        {
            SyncJsonObject.EnterReadLock();
            try
            {
                if (JsonSerializerCache.ContainsKey(type))
                {
                    return JsonSerializerCache[type];
                }
            }
            finally
            {
                SyncJsonObject.ExitReadLock();
            }

            var serializer = new DataContractJsonSerializer(type);
            SyncJsonObject.EnterWriteLock();
            try
            {

                if (!JsonSerializerCache.ContainsKey(type))
                {
                    JsonSerializerCache.Add(type, serializer);
                }
                return serializer;
            }
            finally
            {
                SyncJsonObject.ExitWriteLock();
            }
        }
    }
}
