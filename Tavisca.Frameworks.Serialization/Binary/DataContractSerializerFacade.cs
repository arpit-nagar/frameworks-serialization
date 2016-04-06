using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tavisca.Frameworks.Serialization.Binary
{
    public sealed class DataContractSerializerFacade : ISerializationFacade
    {
        public byte[] Serialize(object obj, object serializationSetting = null)
        {
            if (obj == null)
                return null;

            var type = obj.GetType();

            var serializer = GetDataContractSerializer(type);
            using (var memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, obj);

                var data = new byte[memoryStream.Length];

                Array.Copy(memoryStream.GetBuffer(), data, data.Length);

                return data;
            }
        }

        public T Deserialize<T>(byte[] data, object deserializationSetting = null)
        {
            if (data == null)
                return default(T);

            var type = typeof(T);

            using (var memoryStream = new MemoryStream(data))
            {
                var dcs = GetDataContractSerializer(type);

                return (T)dcs.ReadObject(memoryStream);
            }
        }

        public T DeepClone<T>(T obj)
        {
            var serialized = Serialize(obj);

            var copy = Deserialize<T>(serialized);

            return copy;
        }

        private static readonly Dictionary<Type, DataContractSerializer> DataContractSerializerCache =
            new Dictionary<Type, DataContractSerializer>();
        private static readonly ReaderWriterLockSlim SyncDataContractObject = new ReaderWriterLockSlim();

        private static DataContractSerializer GetDataContractSerializer(Type type)
        {
            SyncDataContractObject.EnterReadLock();
            try
            {
                if (DataContractSerializerCache.ContainsKey(type))
                {
                    return DataContractSerializerCache[type];
                }
            }
            finally
            {
                SyncDataContractObject.ExitReadLock();
            }

            SyncDataContractObject.EnterWriteLock();

            try
            {
                var serializer = new DataContractSerializer(type);
                DataContractSerializerCache.Add(type, serializer);
                return serializer;
            }
            finally
            {
                SyncDataContractObject.ExitWriteLock();
            }
        }
    }
}
