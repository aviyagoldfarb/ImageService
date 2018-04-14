using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public class CommandRecievedEventArgs : EventArgs
    {
        // The command ID
        public int CommandID { get; set; }
        // The arguments for the command
        public string[] Args { get; set; }
        // The requested directory
        public string RequestDirPath { get; set; }

        public CommandRecievedEventArgs(int id, string[] args, string path)
        {
            CommandID = id;
            Args = args;
            RequestDirPath = path;
        }
    }
}
