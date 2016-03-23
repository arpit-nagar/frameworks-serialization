using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavisca.Frameworks.Serialization.Exceptions
{
    [Serializable]
    public class SerializationException : Exception
    {
        public SerializationException() { }

        public SerializationException(string message) : base(message) { }

        public SerializationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
