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
        public void HandleClient(List<TcpClient> clients, TcpClient client, ImageServer server)
        {
            bool resultSuccesful;
            string result;
            bool stop = false;

            new Task(() =>
            {
                do {
                    NetworkStream stream = client.GetStream();
                    BinaryReader reader = new BinaryReader(stream);
                    BinaryWriter writer = new BinaryWriter(stream);

                    //string entireCommand = reader.ReadLine();
                    string command = reader.ReadString();
                    string[] commandAndArg = command.Split(' ');

                    // here supose to be the code that getting the data in the format that we using.

                    if (commandAndArg[0] == "RemoveHandler")
                    {

                        result = server.RemoveHandler(commandAndArg[1]);
                        if (result == "sucsses")
                        {
                            foreach (TcpClient c in clients)
                            {
                                NetworkStream tempStream = c.GetStream();
                                BinaryWriter tempWriter = new BinaryWriter(tempStream);
                                try
                                {
                                    tempWriter.Write("RemoveHandler" + ' ' + commandAndArg[1]);
                                } catch (Exception)
                                {
                                    this.RemoveClient(clients, c);
                                }
                            }
                        }
                    } else if (commandAndArg[0] == "close")
                    {
                        this.RemoveClient(clients, client);
                    } else
                    {
                        string[] args = new string[1];
                        // args[0] = commandAndArg[1];
                        args[0] = " ";
                        result = server.GetController().ExecuteCommand(Convert.ToInt32((Enum.Parse(typeof(CommandEnum), commandAndArg[0]))), args, out resultSuccesful);
                        try
                        {
                            writer.Write(result);
                        } catch (Exception)
                        {
                            this.RemoveClient(clients, client);
                        }
                    }
                   // writer.Write(result);
                } while (!stop);
                client.Close();
            }).Start();
        } 

        public void RemoveClient(List<TcpClient> clients,TcpClient client)
        {
            client.Close();
            clients.Remove(client);
        }

        public void LogClients(List<TcpClient> clients, string message)
        {
            foreach(TcpClient c in clients)
            {
                NetworkStream tempStream = c.GetStream();
                BinaryWriter tempWriter = new BinaryWriter(tempStream);
                try
                {
                    tempWriter.Write("Log" + ' ' + message);
                }
                catch (Exception)
                {
                    this.RemoveClient(clients, c);
                }
            }
        }
    }
}
