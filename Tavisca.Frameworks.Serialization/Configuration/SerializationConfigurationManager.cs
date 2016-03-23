using System;
using Tavisca.Frameworks.Serialization.Resources;

namespace Tavisca.Frameworks.Serialization.Configuration
{
    public static class SerializationConfigurationManager
    {
        private static IApplicationSerializationConfiguration _configuration;

        public static IApplicationSerializationConfiguration GetConfiguration()
        {
            if (_configuration != null)
                return _configuration;

            try
            {
                var config = (ApplicationSerializationSection)
                             System.Configuration.ConfigurationManager.GetSection("ApplicationSerialization");

                _configuration = config ?? new ApplicationSerializationSection();

                return _configuration;
            }
            catch(Exception ex)
            {
                throw new Exceptions.SerializationConfigurationException(
                    SerializationResources.ConfigurationError_Generic, ex);
            }
        }

        public static void SetConfiguration(IApplicationSerializationConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
