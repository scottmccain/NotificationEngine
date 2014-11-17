using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;

namespace NotifierService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            var servicesToRun = new ServiceBase[]
            {
                new NotificationService()
            };
            if (args.Length != 1 || args[0] != "/DEBUG")
            {
                ServiceBase.Run(servicesToRun);
            }
            else
            {
                RunInteractive(servicesToRun);
            }
        }

        static void RunInteractive(IEnumerable<ServiceBase> servicesToRun)
        {
            Console.WriteLine("Services running in interactive mode.");
            Console.WriteLine();

            MethodInfo onStartMethod = typeof(ServiceBase).GetMethod("OnStart",
                BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (ServiceBase service in servicesToRun)
            {
                Console.Write("Starting {0}...", service.ServiceName);
                onStartMethod.Invoke(service, new object[] { new string[] { } });
                Console.Write("Started");
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(
                "Press any key to stop the services and end the process...");
            //Console.ReadLine();
            //Console.WriteLine();

            //MethodInfo onStopMethod = typeof(ServiceBase).GetMethod("OnStop",
            //    BindingFlags.Instance | BindingFlags.NonPublic);
            //foreach (ServiceBase service in servicesToRun)
            //{
            //    Console.Write("Stopping {0}...", service.ServiceName);
            //    onStopMethod.Invoke(service, null);
            //    Console.WriteLine("Stopped");
            //}

            //Console.WriteLine("All services stopped.");
            // Keep the console alive for a second to allow the user to see the message.

            while (true)
            {
                Thread.Sleep(100);

                if (Console.KeyAvailable)
                {
                    break;
                }
            }

            MethodInfo onStopMethod = typeof(ServiceBase).GetMethod("OnStop",
                BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (ServiceBase service in servicesToRun)
            {
                Console.Write("Stopping {0}...", service.ServiceName);
                onStopMethod.Invoke(service, null);
                Console.WriteLine("Stopped");
            }
        }
    }
}
