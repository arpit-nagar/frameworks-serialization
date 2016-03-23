using System;
using System.Collections.Generic;
using System.Configuration;

namespace Tavisca.Frameworks.Serialization.Configuration
{
    public interface IApplicationSerializationConfiguration
    {
        SerializerType DefaultSerializationProvider { get; set; }

        ICollection<ITypeElement> TypeElements { get; set; }

        IEnumerable<IAssemblyElement> GetAssemblyNameByType(Type type);
        ITypeElement GetTypeConfigurationByType(Type type);
    }
}