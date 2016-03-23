using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavisca.Frameworks.Serialization.Configuration
{
    public class SerializationConfiguration : IApplicationSerializationConfiguration
    {
        public SerializerType DefaultSerializationProvider { get; set; }
        public ICollection<ITypeElement> TypeElements { get; set; }
        
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
