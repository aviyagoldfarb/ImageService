using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public interface IImageController
    {
        // Executing the requeted command
        string ExecuteCommand(int commandID, string[] args, out bool result);
    }
}
