using ImageService.Controller;
using ImageService.Infrastructure.Enums;
using ImageService.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{
    class ClientHandler : IClientHandler
    {
        public void HandleClient(TcpClient client, ImageServer server)
        {
            bool resultSuccesful;
            string result;

            new Task(() =>
            {
                NetworkStream stream = client.GetStream();
                BinaryReader reader = new BinaryReader(stream);
                BinaryWriter writer = new BinaryWriter(stream);

                //string entireCommand = reader.ReadLine();
                string command = reader.ReadString();

                // here supose to be the code that getting the data in the format that we using.

                if (command == "RemoveHandler")
                {
                        result = server.RemoveHandler("some path");
                } else
                {
                    string[] args = new string[1];
                    args[0] = "";
                    result = server.GetController().ExecuteCommand(Convert.ToInt32((Enum.Parse(typeof(CommandEnum), command))), args, out resultSuccesful);
                }
                writer.Write(result);
                
                client.Close();
            }).Start();
        }
    }
}
