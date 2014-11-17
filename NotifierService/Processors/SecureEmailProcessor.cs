using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using log4net;
using Notification;
using Notification.Email;
using NotifierService.Infrastructure;

namespace NotifierService.Processors
{
    public class SecureEmailProcessor : IProcessor
    {
        private SmtpClient _client;

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public SecureEmailProcessor()
        {
        }

        public SecureEmailProcessor(SecureEmailProcessorArguments args)
        {
            Initialize(args);
        }

        public bool Process(object value)
        {
            var notification = value as EmailNotification;
            if (notification == null) return false;

            var msg = new MailMessage
            {
                From = new MailAddress(notification.Sender.Address, notification.Sender.DisplayName),
                Subject = notification.Subject,
                Body = notification.Body,
                IsBodyHtml = true
            };

            notification.RecipientList
                .Select(p => new MailAddress(p.Address, p.DisplayName)).ToList()
                .ForEach(p => msg.To.Add(p));

            try
            {
                _client.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug(ex);
            }

            return false;
        }

        public void Initialize(Dictionary<string, string> parameters)
        {
            var args = new SecureEmailProcessorArguments();

            if (parameters.ContainsKey("host"))
            {
                args.Host = parameters["host"];
            }

            if (parameters.ContainsKey("port"))
            {
                args.Port = parameters["port"].SafeConvert<Int32>();
            }

            if (parameters.ContainsKey("user"))
            {
                args.UserName = parameters["user"];
            }

            if (parameters.ContainsKey("password"))
            {
                args.Password = parameters["password"];
            }

            Initialize(args);
        }

        private void Initialize(SecureEmailProcessorArguments args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            _client = new SmtpClient(args.Host, args.Port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(args.UserName, args.Password),
            };
        }
    }
}
