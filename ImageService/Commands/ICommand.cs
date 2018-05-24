using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public interface ICommand
    {
        // The function that will execute the commands
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">the arguments for the executed</param>
        /// <param name="result">the result of the execute (true/false)</param>
        /// <returns></returns>
        string Execute(string[] args, out bool result);
    }
}
