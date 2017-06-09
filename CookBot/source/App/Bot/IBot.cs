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
        string HandleCommand(string message);
    }
}
