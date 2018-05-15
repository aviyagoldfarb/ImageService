using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.ImageService.Commands
{
    class LogCommand : ICommand
    {
        

        public LogCommand()
        {
            
        }

        /// <summary>
        /// Calls AddFile function of ImageServiceModal class
        /// </summary>
        /// <param name="args">The path of the Image from the file</param>
        /// <param name="result">Result of AddFile function</param>
        /// <returns>Message for the log</returns>
        public string Execute(string[] args, out bool result)
        {
           
        }
    }
}
