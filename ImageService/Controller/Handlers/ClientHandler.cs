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
            List<TcpClient> clients = server.GetClients();
            bool stop = false;

            new Task(() =>
            {
                do {
                    NetworkStream stream = client.GetStream();
                    BinaryReader reader = new BinaryReader(stream);
                    BinaryWriter writer = new BinaryWriter(stream);
                    
                    try
                    {
                        string command = reader.ReadString();
                        string[] commandAndArg = command.Split(' ');

                        switch (commandAndArg[0])
                        {
                            case "RemoveHandler":
                                this.RemoveHandler(server, commandAndArg);
                                break;
                            case "Close":
                                this.RemoveClient(clients, client);
                                break;
                            default:
                                this.ExecuteCommand(server, commandAndArg, writer, client);
                                break;
                        }
                    }
                    catch (Exception)
                    {
                        break;
                    }
                } while (!stop);
                client.Close();
            }).Start();
        }

        private void RemoveClient(List<TcpClient> clients, TcpClient client)
        {
            clients.Remove(client);
            client.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="server"></param>
        /// <param name="commandAndArg"></param>
        private void RemoveHandler(ImageServer server, string[] commandAndArg)
        {
            List<TcpClient> clients = server.GetClients();
            string result = server.RemoveHandler(commandAndArg[1]);
            if (result == "sucsses")
            {
                foreach (TcpClient c in clients)
                {
                    NetworkStream tempStream = c.GetStream();
                    BinaryWriter tempWriter = new BinaryWriter(tempStream);
                    try
                    {
                        tempWriter.Write("RemovedHandler" + '#' + commandAndArg[1]);
                    }
                    catch (Exception)
                    {
                        this.RemoveClient(clients, c);
                    }
                }
            }
        }

        private void ExecuteCommand(ImageServer server, string[] commandAndArg, BinaryWriter writer, TcpClient client)
        {
            List<TcpClient> clients = server.GetClients();
            bool resultSuccesful;
            string result;
            string[] args = new string[1];
            args[0] = " ";
            result = server.GetController().ExecuteCommand(Convert.ToInt32((Enum.Parse(typeof(CommandEnum), commandAndArg[0]))), args, out resultSuccesful);
            try
            {
                writer.Write(result);
            }
            catch (Exception)
            {
                this.RemoveClient(clients, client);
            }
        }

        public void LogClients(List<TcpClient> clients, string message)
        {
            new Task(() =>
            {
                foreach (TcpClient c in clients)
                {
                    NetworkStream tempStream = c.GetStream();
                    BinaryWriter tempWriter = new BinaryWriter(tempStream);
                    try
                    {
                        tempWriter.Write("LogUpdated" + '#' + message);
                    }
                    catch (Exception)
                    {
                        this.RemoveClient(clients, c);
                    }
                }
            }).Start();
        }
    }
}
