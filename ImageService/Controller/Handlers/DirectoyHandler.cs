using ImageService.Modal;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System.Text.RegularExpressions;

namespace ImageService.Controller.Handlers
{
    public class DirectoyHandler : IDirectoryHandler
    {
        #region Members
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;
        private FileSystemWatcher m_dirWatcher;             // The Watcher of the Dir
        private string m_path;                              // The Path of directory
        private string[] extentions = { ".jpg", ".png", ".gif", ".bmp" };
        #endregion

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed

        public DirectoyHandler(string path, IImageController controller, ILoggingService logging)
        {
            this.m_controller = controller;
            this.m_path = path;
            this.m_logging = logging;
            this.m_dirWatcher = new FileSystemWatcher(path);
        }

        // The Function Recieves the directory to Handle
        public void StartHandleDirectory(string dirPath)
        {
            this.m_dirWatcher.Created += new FileSystemEventHandler(OnFileMoved);
            this.m_dirWatcher.EnableRaisingEvents = true;
        }

        private void OnFileMoved(object source, FileSystemEventArgs e)
        {
            string[] args = new string[1];
            args[0] = this.m_path;

            string extention = Path.GetExtension(e.FullPath);
            foreach (string ex in this.extentions)
            {
                if (ex == extention)
                {
                    OnCommandRecieved(this, new CommandRecievedEventArgs(0, args, this.m_path));
                }
            }  
        }

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            if (e.CommandID == 0)
            {
                bool result;
                string messsage = this.m_controller.ExecuteCommand(e.CommandID, e.Args, out result);
                if (result)
                {
                    m_logging.Log(messsage, MessageTypeEnum.INFO);
                }
                else
                {
                    m_logging.Log(messsage, MessageTypeEnum.FAIL);
                }
            } else if (e.CommandID == 1)
            {
                m_logging.Log("Handler closing", MessageTypeEnum.INFO);
                this.CloseHandler(e.Args[0]);
            }
        }
        public void CloseHandler(string path)
        {
            this.m_dirWatcher.EnableRaisingEvents = false;
            DirectoryClose?.Invoke(this, new DirectoryCloseEventArgs(path, "Handler closing"));
        }
    }
}
