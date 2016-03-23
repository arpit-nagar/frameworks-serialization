using System;
using System.Collections.Generic;
using System.Configuration;
using Tavisca.Frameworks.Serialization.Compression;

namespace Tavisca.Frameworks.Serialization.Configuration
{
    public sealed class TypeElement : ConfigurationElement, ITypeElement
    {
        [ConfigurationProperty("provider", DefaultValue = SerializerType.RootDefaultOrNone, IsRequired = false)]
        public SerializerType SerializationProvider
        {
            get { return (SerializerType)this["provider"]; }
            set { this["provider"] = value; }
        }

        [ConfigurationProperty("compressionOptions", DefaultValue = CompressionTypeOptions.Deflate, IsRequired = false)]
        public CompressionTypeOptions CompressionOptions
        {
            get { return (CompressionTypeOptions)this["compressionOptions"]; }
            set { this["compressionOptions"] = value; }
        }

        [ConfigurationProperty("type", IsKey = true, IsRequired = true)]
        public string Type
        {
            get { return (string)this["type"]; }
            set { this["type"] = value; }
        }

        private List<IAssemblyElement> _lookupAssemblies;
        public ICollection<IAssemblyElement> LookupAssemblies
        {
            get
            {
                if (_lookupAssemblies == null)
                {
                    _lookupAssemblies = new List<IAssemblyElement>(LookupAssembliesInternal.Count);

                    foreach (var lookupAssembly in LookupAssembliesInternal)
                    {
                        _lookupAssemblies.Add((IAssemblyElement)lookupAssembly);
                    }
                }

                return _lookupAssemblies;
            }
            set
            {
                throw new InvalidOperationException("The collection is read-only.");
            }
        }

        [ConfigurationProperty("lookupAssemblies", IsDefaultCollection = true)]
        public AssemblyElementCollection LookupAssembliesInternal
        {
            get { return (AssemblyElementCollection)this["lookupAssemblies"]; }
            set { this["lookupAssemblies"] = value; }
        }

        public bool Match(Type type)
        {
            return Type.MatchTypeName(type);
        }
    }
}
