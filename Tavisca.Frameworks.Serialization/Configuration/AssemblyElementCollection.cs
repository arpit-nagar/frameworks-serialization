using System.Configuration;

namespace Tavisca.Frameworks.Serialization.Configuration
{
    [ConfigurationCollection(typeof(AssemblyElement))]
    public sealed class AssemblyElementCollection : ConfigurationElementCollection
    {
        #region ConfigurationElementCollection Members

        protected override ConfigurationElement CreateNewElement()
        {
            return new AssemblyElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AssemblyElement)element).Name;
        }

        #endregion
    }
}
