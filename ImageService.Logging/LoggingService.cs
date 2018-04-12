
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {
        // EventHandler MessageRecieved
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        /// <summary>
        /// Invokes the functions that subscribed to MessageRecieved EventHandler
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public void Log(string message, MessageTypeEnum type)
        {
            MessageRecieved?.Invoke(this, new MessageRecievedEventArgs(message, type));
        }
    }
}
