using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace source.App.Commands
{
    public enum BotCode
    {
        Good,
        Bad
    }

    public class BotCommandResult
    {
        public readonly BotCode Code;
        public readonly string Result;

        public BotCommandResult(BotCode code, string result = "")
        {
            Code = code;
            Result = result;
        }
    }
}
