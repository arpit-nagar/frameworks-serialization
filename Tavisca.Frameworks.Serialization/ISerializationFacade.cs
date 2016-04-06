using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavisca.Frameworks.Serialization
{
    /// <summary>
    /// A facade to do serialization and deserialization in binary forms.
    /// </summary>
    public interface ISerializationFacade
    {
        byte[] Serialize(object obj, object serializationSetting = null);
        T Deserialize<T>(byte[] data, object deserializationSetting = null);
        T DeepClone<T>(T obj);
    }
}
