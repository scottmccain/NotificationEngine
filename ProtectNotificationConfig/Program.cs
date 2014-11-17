using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProtectNotificationConfig
{
    class Program
    {
        static void Main(string[] args)
        {
            var sections = new List<string>();
            sections.AddRange(args.Skip(1));

            var exePath = args[0];

            // Open the configuration file and retrieve 
            // the connectionStrings section.
            Configuration config = ConfigurationManager.
                OpenExeConfiguration(exePath);

            var dir = Path.GetDirectoryName(exePath);

            foreach (var sectionName in sections)
            {
                var section = config.GetSection(sectionName);
                if (!section.SectionInformation.IsProtected)
                {
                    section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                    Console.WriteLine("Protecting section {0}", sectionName);
                }
            }

            //var section =
            //    config.GetSection("connectionStrings")
            //    as ConnectionStringsSection;

            //if (section != null)
            //{
            //    if (!section.SectionInformation.IsProtected)
            //    {
            //        // Encrypt the section.
            //        section.SectionInformation.ProtectSection(
            //            "DataProtectionConfigurationProvider");

            //        // Save the current configuration.
            //        config.Save();
            //    }
            //}

            config.Save();
            Console.WriteLine("{0}.config is now protected.", exePath);

        }
    }
}
