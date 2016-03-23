using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavisca.Frameworks.Serialization.Binary;
using Tavisca.Frameworks.Serialization.Compression;
using Tavisca.Frameworks.Serialization.Configuration;

namespace Tavisca.Frameworks.Serialization
{
    public sealed class SerializationFactory
    {
        /// <summary>
        /// Returns the serialization facade which is configuration driven.
        /// </summary>
        public ISerializationFacade GetSerializationFacade()
        {
            return new ConfigDrivenSerializationFacade();
        }

        /// <summary>
        /// Returns the serialization facade which is configuration driven and overrides the serializer.
        /// </summary>
        public ISerializationFacade GetSerializationFacade(SerializerType serializerType)
        {
            return new ConfigDrivenSerializationFacade(serializerType);
        }

        /// <summary>
        /// Returns the serialization facade which is configuration driven, 
        /// overriding both the serializer and compression options.
        /// </summary>
        public ISerializationFacade GetSerializationFacade(SerializerType serializerType, 
            CompressionTypeOptions compression)
        {
            return new ConfigDrivenSerializationFacade(serializerType, compression);
        }
    }
}