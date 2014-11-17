using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Notification;
using Notification.Email;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private MessageQueue messageQueue;

        public Form1()
        {
            InitializeComponent();

            messageQueue = new MessageQueue(@".\Private$\EmailNotifier");
            messageQueue.Formatter = new BinaryMessageFormatter();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var notification = new EmailNotification
            {
                RecipientList = new List<EmailAddress>
                {
                    new EmailAddress {Address = txtName.Text, DisplayName = txtName.Text}
                },

                Subject = txtSubject.Text,
                Body = txtBody.Text,
                Sender = new EmailAddress {Address = "scott.mccain@gmail.com", DisplayName = "Scott McCain"}
            };

            var transaction = new MessageQueueTransaction();

            transaction.Begin();

            var success = false;

            try
            {
                messageQueue.Send(notification, transaction);
                success = true;
            }
            finally
            {
                if(success)
                    transaction.Commit();
                else
                    transaction.Abort();
            }
        }
    }
}
