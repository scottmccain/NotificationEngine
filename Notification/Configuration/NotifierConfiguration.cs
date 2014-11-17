using System.Configuration;

namespace Notification.Configuration
{
    public class NotifierConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("notifiers", IsRequired = true)]
        public NotifierInformationCollection Notifiers
        {
            get { return (NotifierInformationCollection)this["notifiers"]; }
            set { this["notifiers"] = value; }
        }
    }
}
