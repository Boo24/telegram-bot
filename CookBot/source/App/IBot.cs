using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using source.App.Commands;

namespace source.App
{
    interface IBot
    {
        void Run();
        string HandleCommand(string command);
        void SendMessage(string message, long chatId);
    }
}
