using ImageService.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{
    public interface IClientHandler
    {
        void HandleClient(TcpClient client, ImageServer server);
    }
}
