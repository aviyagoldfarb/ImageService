using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Server;
using ImageService.Controller;
using ImageService.Modal;
using System.Configuration;
using ImageService.Controller.Handlers;

namespace ImageService
{
    public partial class ImageService : ServiceBase
    {
        private int eventId = 1;

        private System.ComponentModel.IContainer components;
        private System.Diagnostics.EventLog eventLog1;
        // An ImageServer member
        private ImageServer server;

        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        public ImageService(string[] args)
        {
            InitializeComponent();
            string eventSourceName = ConfigurationManager.AppSettings["SourceName"];
            string logName = ConfigurationManager.AppSettings["LogName"];
            if (args.Count() > 0)
            {
                eventSourceName = args[0];
            }
            if (args.Count() > 1)
            {
                logName = args[1];
            }
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName);
            }
            eventLog1.Source = eventSourceName;
            eventLog1.Log = logName;
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.  
            //eventLog1.WriteEntry("Monitoring the System.", EventLogEntryType.Information, eventId++);
        }

        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            eventLog1.WriteEntry("In OnStart.", EventLogEntryType.Information, eventId++);
            // Set up a timer to trigger every minute.  
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();
            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // In order to create the ImageServiceModal we need to read two fields from the App.config
            string outputFolder = ConfigurationManager.AppSettings["OutputDir"];
            int thumbnailSize = Int32.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);
            // Create an imageModal and a controller 
            IImageServiceModal imageModal = new ImageServiceModal(outputFolder, thumbnailSize);
            IImageController controller = new ImageController(imageModal);
            // Create a logging model
            LoggingService logger = new LoggingService();
            // OnMsg subscribes to MessageRecieved EventHandler
            logger.MessageRecieved += OnMsg;
            IClientHandler ch = new ClientHandler();
            // Create the server
            this.server = new ImageServer(controller, logger, ch);
            // The server creates handlers for each path 
            this.server.CreateHandlers();
        }

        protected override void OnStop()
        {
            // Update the service state to Stop Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            eventLog1.WriteEntry("In onStop.", EventLogEntryType.Information, eventId++);
            // Update the service state to Stopped.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            // The server ends the handlers operation 
            server.SendCommand();
        }
        
        /// <summary>
        /// Invokes by the MessageRecieved EventHandler
        /// </summary>
        /// <param name="sender">The invoker object</param>
        /// <param name="type">MessageRecievedEventArgs</param>
        private void OnMsg(object sender, MessageRecievedEventArgs type)
        {
            switch (type.Status)
            {
                case MessageTypeEnum.INFO:
                    eventLog1.WriteEntry(type.Message, EventLogEntryType.Information, eventId++);
                    break;
                case MessageTypeEnum.WARNING:
                    eventLog1.WriteEntry(type.Message, EventLogEntryType.Warning, eventId++);
                    break;
                case MessageTypeEnum.FAIL:
                    eventLog1.WriteEntry(type.Message, EventLogEntryType.FailureAudit, eventId++);
                    break;
            }
        }

    }
}
