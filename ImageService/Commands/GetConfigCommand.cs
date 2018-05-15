using ImageService.Commands;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public class GetConfigCommand : ICommand
    {

        /// Calls GetConfig function of ImageServiceModal class
        /// </summary>
        /// <param name="args">The  </param>
        /// <param name="result">Result of GetConfig function</param>
        /// <returns>Message for the log</returns>
        public string Execute(string[] args, out bool result)
        {
            // Returns the configurations sending = true, and the error message if result = false
            //return m_modal.GetConfig(out result);

            result = false;
            string paths1 = "Handler$" + ConfigurationManager.AppSettings["Handler"];
            string paths2 = "OutputDir$" + ConfigurationManager.AppSettings["OutputDir"];
            string paths3 = "SourceName$" + ConfigurationManager.AppSettings["SourceName"];
            string paths4 = "LogName$" + ConfigurationManager.AppSettings["LogName"];
            string paths5 = "ThumbnailSize$" + ConfigurationManager.AppSettings["ThumbnailSize"];

            result = true;
            return (paths1 + ' ' + paths2 + ' ' + paths3 + ' ' + paths4 + ' ' + paths5);
        }
    }
}
