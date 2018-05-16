using ImageService.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class LogCommand : ICommand
    {
        private System.Diagnostics.EventLog eventLog;

        public LogCommand(System.Diagnostics.EventLog eventLog)
        {
            this.eventLog = eventLog;
        }

        /// <summary>
        /// Calls AddFile function of ImageServiceModal class
        /// </summary>
        /// <param name="args">The path of the Image from the file</param>
        /// <param name="result">Result of AddFile function</param>
        /// <returns>Message for the log</returns>
        public string Execute(string[] args, out bool result)
        {
            result = false;
            string entireLog = "";
            foreach (EventLogEntry entry in this.eventLog.Entries)
            {
                entireLog += (entry.EntryType + "$" + entry.Message + "\n");
            }
            result = true;
            return entireLog;
        }
    }
}
