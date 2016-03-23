using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavisca.Frameworks.Serialization.Binary;
using Tavisca.Frameworks.Serialization.Compression;
using Tavisca.Frameworks.Serialization.Configuration;
using Tavisca.Frameworks.Serialization.Exceptions;
using Tavisca.Frameworks.Serialization.Resources;

namespace Tavisca.Frameworks.Serialization
{
    internal sealed class ConfigDrivenSerializationFacade : ISerializationFacade
    {
        #region Fields

        private readonly SerializerType? _overrideSerializerType;
        private readonly CompressionTypeOptions? _overrideCompressionTypeOptions;
        private readonly IApplicationSerializationConfiguration _configuration;

        #endregion

        #region Constructors

        public ConfigDrivenSerializationFacade()
        {
            _configuration = SerializationConfigurationManager.GetConfiguration();
        }

        public ConfigDrivenSerializationFacade(SerializerType overrideSerializerType)
            : this()
        {
            _overrideSerializerType = overrideSerializerType;
        }

        public ConfigDrivenSerializationFacade(SerializerType overrideSerializerType,
            CompressionTypeOptions compressionTypeOptions)
            : this(overrideSerializerType)
        {
            _overrideCompressionTypeOptions = compressionTypeOptions;
        }

        #endregion

        #region ISerializationFacade Members

        public byte[] Serialize(object obj)
        {
            if (obj == null)
                throw new SerializationException(string.Format(SerializationResources.Target_Object_Null,
                    SerializationResources.Operation_Serialization));

            ISerializationFacade serializer = null;
            Type objType = null;

            try
            {
                objType = obj.GetType();

                serializer = GetSerializerByObject(objType);

                var serializedData = serializer.Serialize(obj);

                var compressionOption = GetCompressionOption(objType);

                var compressedData = Compress(serializedData, compressionOption);

                return compressedData;
            }
            catch (Exception ex)
            {
                throw CreateException(SerializationResources.Operation_Serialization, serializer, objType, ex);
            }
        }

        public T Deserialize<T>(byte[] data)
        {
            if (data == null)
                return default(T);

            var objType = typeof(T);

            ISerializationFacade serializer = null;
            try
            {
                var compressionOption = GetCompressionOption(objType);

                var decompressedData = DeCompress(data, compressionOption);

                serializer = GetSerializerByObject(objType);

                var obj = serializer.Deserialize<T>(decompressedData);

                return obj;
            }
            catch (Exception ex)
            {
                throw CreateException(SerializationResources.Operation_Deserialization, serializer, objType, ex);
            }
        }

        public T DeepClone<T>(T obj)
        {
            var objType = typeof(T);

            if (obj == null && objType.IsClass)
                throw new SerializationException(string.Format(SerializationResources.Target_Object_Null,
                    SerializationResources.Operation_DeepClone));

            ISerializationFacade serializer = null;
            try
            {
                serializer = GetSerializerByObject(objType);

                return serializer.DeepClone(obj);
            }
            catch (Exception ex)
            {
                throw CreateException(SerializationResources.Operation_DeepClone, serializer, objType, ex);
            }
        }

        #endregion

        #region Private Members

        private ISerializationFacade GetSerializerByObject(Type objType)
        {
            if (_overrideSerializerType.HasValue)
                return GetSerializerByType(_overrideSerializerType.Value);


            var typeConfiguration = _configuration.GetTypeConfigurationByType(objType);

            SerializerType serializerType;
            if (typeConfiguration == null)
            {
                serializerType = GetDefaultSerializerType();
            }
            else
            {
                serializerType = typeConfiguration.SerializationProvider;

                if (serializerType == SerializerType.RootDefaultOrNone)
                    serializerType = GetDefaultSerializerType();
            }

            return GetSerializerByType(serializerType);
        }

        private ISerializationFacade GetSerializerByType(SerializerType serializerType)
        {
            switch (serializerType)
            {
                case SerializerType.Binary:
                    return new BinarySerializationFacade();
                case SerializerType.ProtoBuf:
                    return new ProtoBufFacade();
                case SerializerType.NewtonsoftJsonNetSerializer:
                    return new JsonNetSerializerFacade();
                case SerializerType.DataContractJsonSerializer:
                    return new DataContractJsonSerializerFacade();
                case SerializerType.DataContractSerializer:
                    return new DataContractSerializerFacade();
                case SerializerType.XmlSerializer:
                    return new XmlSerializerFacade();
                default:
                    return new BinarySerializationFacade();
            }
        }

        private SerializerType GetDefaultSerializerType()
        {
            return _configuration.DefaultSerializationProvider;
        }

        private CompressionTypeOptions GetCompressionOption(Type objType)
        {
            if (_overrideCompressionTypeOptions.HasValue)
                return _overrideCompressionTypeOptions.Value;

            var typeConfiguration = _configuration.GetTypeConfigurationByType(objType);

            if (typeConfiguration == null)
            {
                return CompressionTypeOptions.Deflate;
            }

            return typeConfiguration.CompressionOptions;
        }

        private byte[] Compress(byte[] data, CompressionTypeOptions compressionTypeOptions)
        {
            var compressionProvider = GetCompressionProvider();

            return compressionProvider.Compress(data, compressionTypeOptions);
        }

        private byte[] DeCompress(byte[] data, CompressionTypeOptions compressionTypeOptions)
        {
            var compressionProvider = GetCompressionProvider();

            return compressionProvider.DeCompress(data, compressionTypeOptions);
        }

        private CompressionProvider GetCompressionProvider()
        {
            return new CompressionProvider();
        }

        private SerializationException CreateException(string operation,
            ISerializationFacade serializationFacadeType,
            Type obj, Exception original)
        {
            var facadeName = serializationFacadeType == null
                                 ? string.Empty
                                 : serializationFacadeType.GetType().AssemblyQualifiedName;

            var objName = obj == null ? string.Empty : obj.AssemblyQualifiedName;

            return new SerializationException(
                string.Format(SerializationResources.SerializationException_Generic,
                operation, facadeName, objName), original);
        }

        #endregion
    }
}
