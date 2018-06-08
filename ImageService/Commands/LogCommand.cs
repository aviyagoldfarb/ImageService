using ImageService.Commands;
using ImageService.Infrastructure.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    /*
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
            result = false;
            string entireLog = "LogUpdated#";
            EventLogEntry entry;
            EventLogEntryCollection entries = this.eventLog.Entries;
            List<string> entireLogList = new List<string>();

            for (int i = (entries.Count - 1); i > 0; i--)
            {
                entry = entries[i];
                entireLogList.Add((entry.EntryType + "$" + entry.Message + "\n"));
                if (entry.InstanceId.ToString() == "1")
                {
                    break;
                }
            }

            result = true;
            entireLogList.Reverse();
            entireLog += string.Join("", entireLogList.ToArray());
            return entireLog;
        }
    }
    */

    class LogCommand : ICommand
    {
        /// <summary>
        /// The method reads from the machine's EventLog all the relevant
        /// Logs, which are starting with eventID 1.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            Dictionary<int, string[]> logsMap = new Dictionary<int, string[]>();

            string logName = ConfigurationManager.AppSettings["LogName"];
            EventLog myLog = new EventLog(logName, ".");
            EventLogEntry entry;
            EventLogEntryCollection entries = myLog.Entries;

            for (int i = (entries.Count - 1); i > 0; i--)
            {
                entry = entries[i];
                string[] typeAndMessage = new string[2];
                typeAndMessage[0] = entry.EntryType.ToString();
                typeAndMessage[1] = entry.Message.ToString();
                int.TryParse(entry.InstanceId.ToString(), out int id);
                logsMap.Add(id, typeAndMessage);
                if (id == 1)
                {
                    break;
                }
            }

            JObject logObj = new JObject
            {
                ["CommandEnum"] = (int)CommandEnum.LogCommand,
                ["LogMap"] = JsonConvert.SerializeObject(logsMap)
            };
            result = true;
            return logObj.ToString();
        }
    }
}
