using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tavisca.Frameworks.Serialization.Exceptions;
using Tavisca.Frameworks.Serialization.Resources;
using Tavisca.Frameworks.Serialization.Configuration;

namespace Tavisca.Frameworks.Serialization.Binary
{
    public sealed class JsonNetSerializerFacade : ISerializationFacade
    {
        public byte[] Serialize(object obj, object serializationSetting = null)
        {
            if (obj == null)
                return null;

            if (serializationSetting != null)
            {
                var jsonSerializationSetting = serializationSetting as JsonSerializerSettings;

                if (jsonSerializationSetting == null)
                    throw new SerializationSettingException(string.Format(SerializationResources.InvalidSerializationSettingException, SerializerType.NewtonsoftJsonNetSerializer, "JsonSerializerSettings"));

                return JsonConvert.SerializeObject(obj, jsonSerializationSetting).ToBytes();

            }

            return JsonConvert.SerializeObject(obj).ToBytes();
        }

        public T Deserialize<T>(byte[] data, object deserializationSetting = null)
        {
            if (data == null)
                return default(T);

            if (deserializationSetting != null)
            {
                var jsonDeserializationSetting = deserializationSetting as JsonSerializerSettings;

                if (jsonDeserializationSetting == null)
                    throw new SerializationSettingException(string.Format(SerializationResources.InvalidSerializationSettingException, SerializerType.NewtonsoftJsonNetSerializer, "JsonSerializerSettings"));

                return JsonConvert.DeserializeObject<T>(data.FromBytes(), jsonDeserializationSetting);


            }
            return JsonConvert.DeserializeObject<T>(data.FromBytes());
        }

        public T DeepClone<T>(T obj)
        {
            var ser = JsonConvert.SerializeObject(obj);

            var copy = JsonConvert.DeserializeObject<T>(ser);

            return copy;
        }
    }
}
