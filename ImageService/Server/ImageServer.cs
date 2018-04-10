using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        List<IDirectoryHandler> listOfHandlers;
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        #endregion

        public ImageServer(IImageController controller, ILoggingService logging)
        {
            this.m_controller = controller;
            this.m_logging = logging;
            this.listOfHandlers = new List<IDirectoryHandler>();
        }

        public void createHandlers()
        {
            string paths = ConfigurationManager.AppSettings["Handler"];
            string[] listOfPaths = paths.Split(';');
            foreach (string path in listOfPaths)
            {
                IDirectoryHandler handler = new DirectoyHandler(path, this.m_controller, this.m_logging);
                CommandRecieved += handler.OnCommandRecieved;
                handler.DirectoryClose += this.OnCloseServer;
                handler.StartHandleDirectory(path);
                this.listOfHandlers.Add(handler);
            }


        }
        public void SendCommand()
        {
            string[] args = { };
            this.CommandRecieved?.Invoke(this, new CommandRecievedEventArgs(1,args, "here"));
        }

        public void OnCloseServer(object sender, DirectoryCloseEventArgs e)
        {
            IDirectoryHandler handler = (IDirectoryHandler)sender; 
            CommandRecieved -= handler.OnCommandRecieved;
            handler.DirectoryClose -= this.OnCloseServer;
        }

    }
}
