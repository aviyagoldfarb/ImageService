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
        /// <summary>
        ///  the command recieving evevnt
        /// </summary>
        /// <param name="id"> the id of the command</param>
        /// <param name="args">The arguments that needed for executing the command</param>
        /// <param name="path">the path of the directory</param>
        public CommandRecievedEventArgs(int id, string[] args, string path)
        {
            CommandID = id;
            Args = args;
            RequestDirPath = path;
        }
    }
}
