using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tavisca.Frameworks.Serialization.Binary
{
    public sealed class JsonNetSerializerFacade : ISerializationFacade
    {
        public byte[] Serialize(object obj)
        {
            if (obj == null)
                return null;

            return JsonConvert.SerializeObject(obj).ToBytes();
        }

        public T Deserialize<T>(byte[] data)
        {
            if (data == null)
                return default(T);

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
