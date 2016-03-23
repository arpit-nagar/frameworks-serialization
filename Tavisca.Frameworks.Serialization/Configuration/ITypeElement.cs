using System;
using System.Collections.Generic;
using Tavisca.Frameworks.Serialization.Compression;

namespace Tavisca.Frameworks.Serialization.Configuration
{
    public interface ITypeElement
    {
        SerializerType SerializationProvider { get; set; }
        CompressionTypeOptions CompressionOptions { get; set; }
        string Type { get; set; }
        ICollection<IAssemblyElement> LookupAssemblies { get; set; }
        bool Match(Type type);
    }
}