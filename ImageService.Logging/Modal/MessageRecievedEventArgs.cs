﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging.Modal
{
    public class MessageRecievedEventArgs : EventArgs
    {
        public MessageTypeEnum Status { get; set; }
        public string Message { get; set; }

        // EventArgs for the EventHandler MessageRecieved
        public MessageRecievedEventArgs(string message, MessageTypeEnum status)
        {
            Status = status;
            Message = message;
        }
    }
}
