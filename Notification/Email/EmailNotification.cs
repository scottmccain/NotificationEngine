using System;
using System.Collections.Generic;

namespace Notification.Email
{
    [Serializable()]
    public class EmailNotification
    {
        public List<EmailAddress> RecipientList { get; set; }
        public List<EmailAddress> CcList { get; set; }
        public List<EmailAddress> BccList { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }

        public EmailAddress Sender { get; set; }
    }
}
