
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
        // The event that notifies about a message recieved
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        /// <summary>
        /// Invokes the functions that subscribed to MessageRecieved EventHandler
        /// </summary>
        /// <param name="message">The message for the log</param>
        /// <param name="type">The type of the message</param>
        public void Log(string message, MessageTypeEnum type)
        {
            MessageRecieved?.Invoke(this, new MessageRecievedEventArgs(message, type));
        }
    }
}
