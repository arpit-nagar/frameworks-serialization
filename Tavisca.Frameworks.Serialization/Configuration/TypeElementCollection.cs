using System.Configuration;

namespace Tavisca.Frameworks.Serialization.Configuration
{
    [ConfigurationCollection(typeof(TypeElement))]
    public sealed class TypeElementCollection : ConfigurationElementCollection
    {
        #region ConfigurationElementCollection Members

        protected override ConfigurationElement CreateNewElement()
        {
            return new TypeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TypeElement)element).Type;
        }

        #endregion
    }
}
