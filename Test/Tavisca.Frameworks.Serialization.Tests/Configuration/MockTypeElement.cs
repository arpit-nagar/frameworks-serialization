using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavisca.Frameworks.Serialization.Configuration;

namespace Tavisca.Frameworks.Serialization.Tests.Configuration
{
    public class MockTypeElement : ITypeElement
    {
        private SerializerType _serializationProvider;
        private Compression.CompressionTypeOptions _compressionOptions;
        private string _type;
        private ICollection<IAssemblyElement> _lookupAssemblies;

        public SerializerType SerializationProvider
        {
            get { return _serializationProvider; }
            set { _serializationProvider = value; }
        }

        public Compression.CompressionTypeOptions CompressionOptions
        {
            get { return _compressionOptions; }
            set { _compressionOptions = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public ICollection<IAssemblyElement> LookupAssemblies
        {
            get { return _lookupAssemblies; }
            set { _lookupAssemblies = value; }
        }

        public bool Match(Type type)
        {
            return FormatTypeName(type.FullName).Equals(FormatTypeName(Type),
                                                                     StringComparison.InvariantCultureIgnoreCase);
        }

        private string FormatTypeName(string typeName)
        {
            return typeName.Trim().Replace(" ", "");
        }
    }
}