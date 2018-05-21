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
        void HandleClient(List<TcpClient> clients, TcpClient client, ImageServer server);
        void LogClients(List<TcpClient> clients, string message);
    }
}
