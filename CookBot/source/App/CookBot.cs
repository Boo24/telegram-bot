using System.Collections.Generic;
using System.Linq;
using source.Domain.Model;
using source.Infrastructure.Databases;
using source.App.Commands;

namespace source.App
{
    public class CookBot : IBot
    {
        private List<IBotCommand> BotCommands { get; }

        public CookBot(List<IBotCommand> botCommands)
            => BotCommands = botCommands;

        public string HandleCommand(string message)
        {
            var query = message.Split(' ');
            foreach (var command in BotCommands)
            {
                if (command.Name != query[0]) continue;
                var args = query.Skip(1).ToArray();
                var commandResult = command.Execute(args);
                return commandResult.Result;
            }
            var help = GetHelpCommand();
            return help != null ? help.Execute(query.Skip(1).ToArray()).Result : "Неизвестная команда!";
        }

        private IBotCommand GetHelpCommand() => BotCommands.Find(x => x is HelpCommand);
    }
}
