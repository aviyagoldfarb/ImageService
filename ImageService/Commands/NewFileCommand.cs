using ImageService.Infrastructure;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public class NewFileCommand : ICommand
    {
        private IImageServiceModal m_modal;

        public NewFileCommand(IImageServiceModal modal)
        {
            // Storing the Modal
            m_modal = modal;
        }

        /// <summary>
        /// Calls AddFile function of ImageServiceModal class
        /// </summary>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns>Message for the log</returns>
        public string Execute(string[] args, out bool result)
        {
            // Returns the new path if result = true, and the error message if result = false
            return m_modal.AddFile(args[0], out result);
        }
    }
}
