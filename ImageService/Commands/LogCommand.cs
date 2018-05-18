using ImageService.Commands;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class LogCommand : ICommand
    {
        private System.Diagnostics.EventLog eventLog;

        public LogCommand()
        {
            string logName = ConfigurationManager.AppSettings["LogName"];
            this.eventLog = new EventLog(logName, ".");
        }

        /// <summary>
        /// Calls AddFile function of ImageServiceModal class
        /// </summary>
        /// <param name="args">The path of the Image from the file</param>
        /// <param name="result">Result of AddFile function</param>
        /// <returns>Message for the log</returns>
        public string Execute(string[] args, out bool result)
        {
            /*
            result = false;
            string entireLog = "";
            foreach (EventLogEntry entry in this.eventLog.Entries)
            {
                entireLog += (entry.EntryType + "$" + entry.Message + "\n");
            }
            result = true;
            return entireLog;
            */
            
            result = false;
            string entireLog = "";
            EventLogEntry entry;
            EventLogEntryCollection entries = this.eventLog.Entries;
            for (int i = (entries.Count - 1); i > 0; i--)
            {
                entry = entries[i];
                entireLog += (entry.EntryType + "$" + entry.Message + "\n");
                if (entry.InstanceId.ToString() == "1")
                {
                    break;
                }
            }
            result = true;
            return entireLog;
        }
    }
}
