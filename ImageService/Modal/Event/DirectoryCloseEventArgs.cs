using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public class DirectoryCloseEventArgs : EventArgs
    {
        public string DirectoryPath { get; set; }
        // The Message That goes to the logger
        public string Message { get; set; }

        public DirectoryCloseEventArgs(string dirPath, string message)
        {
            // Setting the Directory Name
            DirectoryPath = dirPath;
            // Storing the String
            Message = message;
        }

    }
}
