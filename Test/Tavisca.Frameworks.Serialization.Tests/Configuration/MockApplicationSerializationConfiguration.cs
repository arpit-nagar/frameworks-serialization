using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavisca.Frameworks.Serialization.Configuration;
using Tavisca.Frameworks.Serialization.Compression;

namespace Tavisca.Frameworks.Serialization.Tests.Configuration
{

    public class MockApplicationSerializationConfiguration : IApplicationSerializationConfiguration
    {
        public SerializerType DefaultSerializationProvider { get; set; }

        public ICollection<ITypeElement> TypeElements { get; set; }

        public MockApplicationSerializationConfiguration()
        {
            TypeElements = new List<ITypeElement>();
        }
        
        public IEnumerable<IAssemblyElement> GetAssemblyNameByType(Type type)
        {
            if (TypeElements == null)
                yield break;

            ICollection<IAssemblyElement> targetAssemblies = null;

            var typeConfig = GetTypeConfigurationByType(type);

            if (typeConfig != null)
                targetAssemblies = typeConfig.LookupAssemblies;

            if (targetAssemblies == null)
                targetAssemblies = new List<IAssemblyElement>();

            foreach (var obj in targetAssemblies)
            {
                var targetAssembly = obj;

                if (targetAssembly == null)
                    continue;

                yield return targetAssembly;
            }
        }

        public ITypeElement GetTypeConfigurationByType(Type type)
        {
            return this.TypeElements == null ? null :
                this.TypeElements.FirstOrDefault(typeElement => typeElement.Match(type));
        }
    }
}