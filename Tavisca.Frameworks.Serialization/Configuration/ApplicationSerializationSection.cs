using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Tavisca.Frameworks.Serialization.Configuration
{
    public sealed class ApplicationSerializationSection : ConfigurationSection, IApplicationSerializationConfiguration
    {
        [ConfigurationProperty("defaultProvider", DefaultValue = SerializerType.Binary, IsRequired = false)]
        public SerializerType DefaultSerializationProvider
        {
            get { return (SerializerType)this["defaultProvider"]; }
            set { this["defaultProvider"] = value; }
        }

        private List<ITypeElement> _typeElements;
        public ICollection<ITypeElement> TypeElements
        {
            get
            {
                if (_typeElements == null)
                {
                    var typeElements = new List<ITypeElement>(Types.Count);
                    
                    typeElements.AddRange(Types.OfType<ITypeElement>().Where(x => x != null));

                    typeElements.TrimExcess();
                    
                    _typeElements = typeElements;
                }
                return _typeElements;
            }
            set
            {
                throw new InvalidOperationException("The collection is read-only.");
            }
        }

        [ConfigurationProperty("types", IsDefaultCollection = true)]
        public TypeElementCollection Types
        {
            get { return (TypeElementCollection)this["types"]; }
            set { this["types"] = value; }
        }

        public IEnumerable<IAssemblyElement> GetAssemblyNameByType(Type type)
        {
            return this.GetAssembliesFromType(type);
        }

        public ITypeElement GetTypeConfigurationByType(Type type)
        {
            return this.GetTypeConfigurationByTypeFromConfig(type);
        }
    }
}
