using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavisca.Frameworks.Serialization.Exceptions
{
    [Serializable]
    public class SerializationConfigurationException : SerializationException
    {
        public SerializationConfigurationException() { }

        public SerializationConfigurationException(string message) : base(message) { }

        public SerializationConfigurationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
