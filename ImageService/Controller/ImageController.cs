using ImageService.Commands;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        // The Modal object
        private IImageServiceModal m_modal;
        private Dictionary<int, ICommand> commands;

        public ImageController(IImageServiceModal modal)
        {
            // Storing the Modal of the system
            m_modal = modal;
            commands = new Dictionary<int, ICommand>();
            commands.Add((int)(CommandEnum.NewFileCommand), new NewFileCommand(modal));
        }

        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            if (!commands.ContainsKey(commandID))
            {
                resultSuccesful = false;
                return "command not found";
            }

            // Create an instance of a Task 
            Task<Tuple<string, bool>> task = new Task<Tuple<string, bool>>(() =>
            {
                bool resultSuccesfulTemp;
                // Eexecute the relevant command according to the commandID
                string message = commands[commandID].Execute(args, out resultSuccesfulTemp);
                return Tuple.Create(message, resultSuccesfulTemp);
            });
            task.Start();
            Tuple<string, bool> result = task.Result;
            resultSuccesful = result.Item2;
            return result.Item1;
        }
    }
}
