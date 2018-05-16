using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        private List<IDirectoryHandler> listOfHandlers;
        private TcpListener listener;
        private IClientHandler ch;
        private TcpClient client;
        #endregion

        #region Properties
        // The event that notifies about a new Command being recieved
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;

        #endregion

        public ImageServer(IImageController controller, ILoggingService logging, IClientHandler ch)
        {
            this.m_controller = controller;
            this.m_logging = logging;
            this.listOfHandlers = new List<IDirectoryHandler>();
            this.ch = ch;
        }

        public void Start()
        {
            //this.m_logging.Log("before communicationConfig", MessageTypeEnum.INFO);
            
            //string[] communicationConfig = System.IO.File.ReadAllLines(@"C:\Users\hana\source\repos\ImageService\ImageService\communicationConfig.txt");

            //this.m_logging.Log("after communicationConfig", MessageTypeEnum.INFO);

            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7500);
            //IPEndPoint ep = new IPEndPoint(IPAddress.Parse(communicationConfig[0]), int.Parse(communicationConfig[1]));
            listener = new TcpListener(ep);
            listener.Start();
            // Waiting for connections, each connection in a new thread
            Task task = new Task(() => {
                while (true)
                {
                    try
                    {
                        this.client = listener.AcceptTcpClient();
                        // Got a new connection
                        ch.HandleClient(client, this);
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
                // Server stopped
            });
            task.Start();
        }

        public void StopListen()
        {
            listener.Stop();
        }
        
        /// <summary>
        /// Creates handler for each specified path in the App.config 
        /// </summary>
        public void CreateHandlers()
        {
            string paths = ConfigurationManager.AppSettings["Handler"];
            string[] listOfPaths = paths.Split(';');
            foreach (string path in listOfPaths)
            {
                IDirectoryHandler handler = new DirectoyHandler(path, this.m_controller, this.m_logging, this.client);
                // handler.OnCommandRecieved subscribes to CommandRecieved EventHandler
                CommandRecieved += handler.OnCommandRecieved;
                // OnCloseServer subscribes to DirectoryClose EventHandler
                handler.DirectoryClose += OnCloseServer;
                // Creates FileSystemEventHandler to track after any creation of a file or directory in the specified path
                handler.StartHandleDirectory(path);
                this.listOfHandlers.Add(handler);
            }
            this.Start();
        }

        public string RemoveHandler(string path)
        {
            foreach (IDirectoryHandler handler in this.listOfHandlers)
            {
                 if (handler.getPath() == path)
                {
                    CommandRecieved -= handler.OnCommandRecieved;
                    return ("sucsses");
                }
             }
            return ("failed");
        }

        /// <summary>
        /// Invokes the functions that subscribed to CommandRecieved EventHandler
        /// </summary>
        public void SendCommand()
        {
            string[] args = new string[1];
            args[0] = "";
            CommandRecieved?.Invoke(this, new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, args, ""));
        }

        /// <summary>
        /// Invokes by the handler.DirectoryClose EventHandler 
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">DirectoryCloseEventArgs</param>
        public void OnCloseServer(object sender, DirectoryCloseEventArgs e)
        {
            IDirectoryHandler handler = (IDirectoryHandler)sender;
            // handler.OnCommandRecieved removes itself from CommandRecieved EventHandler
            CommandRecieved -= handler.OnCommandRecieved;
            // OnCloseServer removes itself from DirectoryClose EventHandler
            handler.DirectoryClose -= OnCloseServer;
            // Remove this handler from the listOfHandlers
            this.listOfHandlers.Remove(handler);
        }

        public IImageController GetController() {
            return this.m_controller;
        }
    }
}
