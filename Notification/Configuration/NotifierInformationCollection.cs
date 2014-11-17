using System.Configuration;

namespace Notification.Configuration
{
    public class NotifierInformationCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new NotifierInformationElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((NotifierInformationElement)element).Name;
        }
    }
}