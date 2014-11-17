using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Messaging;
using Notification;

namespace NotifierService
{
    public partial class NotificationService : ServiceBase
    {
        public NotificationService()
        {
            InitializeComponent();
        }

        //protected bool Running { get; set; }

        //private MSMQNotifierEngine<EmailNotification> _emailEngine;

        private ServiceController _controller = new ServiceController();

        protected override void OnStart(string[] args)
        {

            _controller.Start(args);
            //Running = true;

            //messageQueue.Formatter = new BinaryMessageFormatter();
            //messageQueue.ReceiveCompleted += MessageQueueOnReceiveCompleted;
            //messageQueue.BeginReceive(TimeSpan.FromMilliseconds(250));


        }

        //private void MessageQueueOnReceiveCompleted(object source, ReceiveCompletedEventArgs asyncResult)
        //{
        //    try
        //    {
        //        var mq = (MessageQueue)source;

        //        if (mq != null)
        //        {
        //            try
        //            {
        //                System.Messaging.Message message = null;
        //                try
        //                {
        //                    message = mq.EndReceive(asyncResult.AsyncResult);
        //                }
        //                catch (Exception ex)
        //                {
        //                    //LogMessage(ex.Message);
        //                }
        //                if (message != null)
        //                {
        //                    EmailNotification payload = message.Body as EmailNotification;
        //                    if (payload != null)
        //                    {
        //                    }
        //                }
        //            }
        //            finally
        //            {
        //                if (Running)
        //                {
        //                    mq.BeginReceive(TimeSpan.FromMilliseconds(250));
        //                }
        //            }
        //        }
        //        return;
        //    }
        //    catch (Exception exc)
        //    {
        //        //LogMessage(exc.Message);
        //    }
        //}

        protected override void OnStop()
        {
            //Running = false;

            _controller.Stop();
        }
    }
}
