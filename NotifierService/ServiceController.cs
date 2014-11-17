using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Messaging;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using Notification;
using Notification.Configuration;
using NotifierService.Processors;

namespace NotifierService
{
    public class ServiceController
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ManualResetEvent _stopEvent = new ManualResetEvent(false);
        private readonly List<MSMQNotifierEngine> _notifiers = new List<MSMQNotifierEngine>();
 
        public void Start(string [] args)
        {
            var config =
                (NotifierConfiguration)ConfigurationManager.GetSection(
                "notifierConfiguration");

            var thread = new Thread(() =>
            {
                LoadEngines(config);

                foreach (var engine in _notifiers)
                {
                    engine.StartEngine();
                }

                _stopEvent.WaitOne();

                foreach (var engine in _notifiers)
                {
                    engine.StopEngine();
                }
            });

            Running = true;
            thread.Start();
        }

        private void LoadEngines(NotifierConfiguration config)
        {
            foreach (NotifierInformationElement notifier in config.Notifiers)
            {
                var processor = GetProcessorFromTypeString(notifier.ProcessorType);
                if (processor == null) continue;

                var parms =
                    notifier.
                        Parameters.
                        Cast<NotifierParameterElement>().ToDictionary(parm => parm.Name, parm => parm.Value);

                processor.Initialize(parms);

                var engine = new MSMQNotifierEngine(notifier.QueuePath, processor);
                _notifiers.Add(engine);
            }
        }

        private static IProcessor GetProcessorFromTypeString(string typeString)
        {
            var processorInfo = typeString.Split(',').Select(p => p.Trim()).ToArray();

            if (processorInfo.Length != 2) return null;

            try
            {
                return Activator.CreateInstance(processorInfo[1], processorInfo[0]).Unwrap() as IProcessor;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return null;
        }

        public void Stop()
        {
            Running = false;
            _stopEvent.Set();
        }

        private bool _running;

        protected bool Running
        {
            get
            {
                
                Thread.MemoryBarrier();
                try
                {
                    return _running;
                }
                finally 
                {
                    Thread.MemoryBarrier();
                }
                
            }

            set
            {
                Thread.MemoryBarrier();
                _running = value;
                Thread.MemoryBarrier();
            }
        }

    }
}
