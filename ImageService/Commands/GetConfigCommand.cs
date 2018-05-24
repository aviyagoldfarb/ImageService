﻿using ImageService.Commands;
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
            string path1 = "Handler$" + ConfigurationManager.AppSettings["Handler"];
            string path2 = "OutputDir$" + ConfigurationManager.AppSettings["OutputDir"];
            string path3 = "SourceName$" + ConfigurationManager.AppSettings["SourceName"];
            string path4 = "LogName$" + ConfigurationManager.AppSettings["LogName"];
            string path5 = "ThumbnailSize$" + ConfigurationManager.AppSettings["ThumbnailSize"];

            result = true;
            return ("ConfigRecieved#" + path1 + '\n' + path2 + '\n' + path3 + '\n' + path4 + '\n' + path5);
        }
    }
}
