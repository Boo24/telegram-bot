using System;
using System.Collections.Generic;
using System.Linq;
using source.Domain.Model;
using source.Infrastructure.Databases;
using source.App.Commands;

namespace source.App
{
    public class CookBot : IBot
    {
        public IDatabase<Recipe> Database { get; }
        public List<IBotCommand> BotCommands { get; }

        public CookBot(IDatabase<Recipe> database, List<IBotCommand> botCommands)
        {
            Database = database;
            BotCommands = botCommands;
        }

        public string HandleCommand(string message)
        {
            var query = message.Split(' ');
            foreach (var command in BotCommands)
            {
                if (command.Name != query[0]) continue;
                var result = command.Execute(Database, query.Skip(1).ToArray());
                if (result == BotCommandResult.Bad)
                    return "Нет подходящих рецептов :(";
                return command.Result;

            }
            var help = GetHelpCommand();
            return help != null ? help.Result : "Неизвестная команда!";
        }
        private IBotCommand GetHelpCommand()
            => BotCommands.Find(x => x is HelpCommand);
    }
}
