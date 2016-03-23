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
        byte[] Serialize(object obj);
        T Deserialize<T>(byte[] data);
        T DeepClone<T>(T obj);
    }
}
