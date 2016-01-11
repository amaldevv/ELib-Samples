using Elib.Core.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elib
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bootstrapping logger");
            BootStrapLogger();
            Console.WriteLine("Logging");
            for(int i=0;i<1000;i++)
            {
                LogEntry entry = new LogEntry()
                {
                    Message = "This is a debug info",
                    Severity = System.Diagnostics.TraceEventType.Verbose
                };
                Logger.Write(entry);

                entry = new LogEntry()
                {
                    Message = "This is a warning message",
                    Severity = System.Diagnostics.TraceEventType.Warning
                };
                Logger.Write(entry);

                entry = new LogEntry()
                {
                    Message = "This is a info message",
                    Severity = System.Diagnostics.TraceEventType.Information
                };
                Logger.Write(entry);

                entry = new LogEntry()
                {
                    
                    Message = i.ToString() + " -> This is a critical message",
                    Severity = System.Diagnostics.TraceEventType.Critical
                };
                Logger.Write(entry);
            }
            Console.WriteLine("Finished logging");

            Console.ReadKey();
        }

        private static void BootStrapLogger()
        {
            IConfigurationSource configSource = ConfigurationSourceFactory.Create();

            var loggingSettings = configSource.GetSection(LoggingSettings.SectionName) as LoggingSettings;

            var data = loggingSettings.TraceListeners.First(t => t.Name == "Rolling Flat File Trace Listener") as RollingFlatFileTraceListenerData;

            // Change the file name
            data.FileName = "modified.log";

            var loggingXmlSrc =new SerializableConfigurationSource();
            loggingXmlSrc.Add(LoggingSettings.SectionName, loggingSettings);

            var logFactory = new LogWriterFactory(loggingXmlSrc);
            Logger.SetLogWriter(logFactory.Create());

        }
    }
}
