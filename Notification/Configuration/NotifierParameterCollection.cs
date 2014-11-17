using System.Configuration;

namespace Notification.Configuration
{
    public class NotifierParameterCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new NotifierParameterElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((NotifierParameterElement)element).Name;
        }
    }
}