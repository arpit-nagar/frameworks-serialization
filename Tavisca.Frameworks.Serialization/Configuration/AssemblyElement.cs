using System.Configuration;

namespace Tavisca.Frameworks.Serialization.Configuration
{
    public sealed class AssemblyElement : ConfigurationElement, IAssemblyElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }
    }
}
