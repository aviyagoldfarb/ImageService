﻿using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    public interface ILoggingService
    {
        // The event that notifies about a message recieved
        event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        // Logging the message
        void Log(string message, MessageTypeEnum type);
    }
}
