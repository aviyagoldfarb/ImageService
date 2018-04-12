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
        // The Image Processing Controller
        private IImageController m_controller;
        private ILoggingService m_logging;
        // The Watcher of the Dir
        private FileSystemWatcher m_dirWatcher;
        // The Path of directory
        private string m_path;
        // Extentions of image files we would like to listen to
        private string[] extentions = { ".jpg", ".png", ".gif", ".bmp" };
        #endregion

        // The Event That Notifies that the Directory is being closed
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;

        public DirectoyHandler(string path, IImageController controller, ILoggingService logging)
        {
            this.m_path = path;
            this.m_controller = controller;
            this.m_logging = logging;
            this.m_dirWatcher = new FileSystemWatcher(path);
        }

        // The Function Recieves the directory to Handle
        public void StartHandleDirectory(string dirPath)
        {
            // When a file or directory in the specified path is created, invoke OnFileMoved function
            this.m_dirWatcher.Created += new FileSystemEventHandler(OnFileMoved);
            this.m_dirWatcher.EnableRaisingEvents = true;
        }

        private void OnFileMoved(object source, FileSystemEventArgs e)
        {
            string[] args = new string[1];
            args[0] = /*this.m_path*/e.FullPath;

            string extention = Path.GetExtension(e.FullPath);
            foreach (string ext in this.extentions)
            {
                if (ext == extention)
                {
                    OnCommandRecieved(this, new CommandRecievedEventArgs((int)CommandEnum.NewFileCommand, args, this.m_path));
                }
            }  
        }

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            if (e.CommandID == (int)CommandEnum.NewFileCommand)
            {
                bool result;
                string messsage = this.m_controller.ExecuteCommand(e.CommandID, e.Args, out result);
                if (result)
                {
                    this.m_logging.Log(messsage, MessageTypeEnum.INFO);
                }
                else
                {
                    this.m_logging.Log(messsage, MessageTypeEnum.FAIL);
                }
            }
            else if (e.CommandID == (int)CommandEnum.CloseCommand)
            {
                this.m_logging.Log("Handler closing", MessageTypeEnum.INFO);
                this.CloseHandler(e.Args[0]);
            }
        }

        public void CloseHandler(string path)
        {
            this.m_dirWatcher.EnableRaisingEvents = false;
            DirectoryClose?.Invoke(this, new DirectoryCloseEventArgs(path, " Close Handler"));
        }
    }
}
