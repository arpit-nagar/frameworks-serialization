using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavisca.Frameworks.Serialization.Exceptions
{
    [Serializable]
    public class SerializationSettingException : SerializationException
    {
        public SerializationSettingException() { }

        public SerializationSettingException(string message) : base(message) { }

        public SerializationSettingException(string message, Exception innerException) : base(message, innerException) { }

    }
}
