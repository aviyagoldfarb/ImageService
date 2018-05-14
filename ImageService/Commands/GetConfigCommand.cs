using ImageService.Commands;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public class GetConfigCommand : ICommand
    {
        IImageServiceModal m_modal;

        public GetConfigCommand(IImageServiceModal modal)
        {
            // Storing the Modal
            m_modal = modal;
        }

        /// Calls GetConfig function of ImageServiceModal class
        /// </summary>
        /// <param name="args">The  </param>
        /// <param name="result">Result of GetConfig function</param>
        /// <returns>Message for the log</returns>
        public string Execute(string[] args, out bool result)
        {
            // Returns the configurations sending = true, and the error message if result = false
            return m_modal.GetConfig(out result);
        }
    }
}
