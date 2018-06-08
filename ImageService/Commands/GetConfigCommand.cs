using ImageService.Commands;
using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    /*
    public class GetConfigCommand : ICommand
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
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
    */

    class GetConfigCommand : ICommand
    {
        /// <summary>
        /// The method parses the App.config of the service and returns a 
        /// JSON string with all the values.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            string handlersPaths = ConfigurationManager.AppSettings["Handler"];
            string outputDir = ConfigurationManager.AppSettings["OutputDir"];
            string srcName = ConfigurationManager.AppSettings["SourceName"];
            string logName = ConfigurationManager.AppSettings["LogName"];
            string thumbnailSize = ConfigurationManager.AppSettings["ThumbnailSize"];

            string[] handlerPaths = handlersPaths.Split(';');

            JObject configObj = new JObject
            {
                ["CommandEnum"] = (int)CommandEnum.GetConfigCommand,
                ["HandlersPaths"] = JsonConvert.SerializeObject(handlerPaths),
                ["OutputDir"] = outputDir,
                ["SourceName"] = srcName,
                ["LogName"] = logName,
                ["ThumbnailSize"] = thumbnailSize
            };
            result = true;
            return configObj.ToString();
        }
    }
}
