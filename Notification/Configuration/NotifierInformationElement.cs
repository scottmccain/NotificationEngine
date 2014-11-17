using System.Configuration;

namespace Notification.Configuration
{
    public class NotifierInformationElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("processorType", IsRequired = true)]
        public string ProcessorType
        {
            get { return (string)base["processorType"]; }
            set { base["processorType"] = value; }
        }

        [ConfigurationProperty("queuePath", IsRequired = true)]
        public string QueuePath
        {
            get { return (string)this["queuePath"]; }
            set { this["queuePath"] = value; }
        }

        [ConfigurationProperty("parameters", IsRequired = true)]
        public NotifierParameterCollection Parameters
        {
            get { return (NotifierParameterCollection)this["parameters"]; }
            set { this["parameters"] = value; }
        }
    }
}
