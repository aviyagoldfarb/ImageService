using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public interface ICommand
    {
        // The function that will execute the commands
        string Execute(string[] args, out bool result);
    }
}
