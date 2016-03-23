using System;
using System.Collections.Generic;
using Tavisca.Frameworks.Serialization.Compression;

namespace Tavisca.Frameworks.Serialization.Configuration
{
    public class TypeElementConfiguration : ITypeElement
    {
        public SerializerType SerializationProvider { get; set; }
        public CompressionTypeOptions CompressionOptions { get; set; }
        public string Type { get; set; }
        public ICollection<IAssemblyElement> LookupAssemblies { get; set; }

        public bool Match(Type type)
        {
            return Type.MatchTypeName(type);
        }
    }
}