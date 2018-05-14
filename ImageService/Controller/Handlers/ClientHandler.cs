using ImageService.Controller;
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
            string[] strCommand;
            int commandID;
            bool resultSuccesful;
            string result;


            new Task(() =>
            {
                using (NetworkStream stream = client.GetStream())
                using (StreamReader reader = new StreamReader(stream))
                using (StreamWriter writer = new StreamWriter(stream))
                {

                    strCommand = reader.ReadLine().Split(' ');
                    commandID = int.Parse(strCommand[0]);

                    /** here supose to be the 
                         code that getting the data
                    in the format that we using.

                       */
                    //  Console.WriteLine("Got command: %d\n", commandId);


                    if (commandID == 5)
                    {
                        result = server.RemoveHandler(strCommand[1]);
                    } else
                    {
                        result = server.GetController().ExecuteCommand(commandID, strCommand, out resultSuccesful);
                    }
                    writer.Write(result);
                }
                client.Close();
            }).Start();
        }
    }
}
