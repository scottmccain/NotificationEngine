using System;

namespace Notification.Email
{
    [Serializable()]
    public class EmailAddress
    {
        public string DisplayName
        {
            get; set;
        }

        public string Address { get; set; }

    }
}
