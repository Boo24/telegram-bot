using System.Collections.Generic;
using System.Linq;
using CookBot.Domain.Model;
using CookBot.Infrastructure.Databases;
using source.App.Commands;

namespace source.App
{
    class CookBot : IBot
    {
        public IDatabase<Recipe> Database { get; }
        public List<IBotCommand> Commands { get; }

        public CookBot(IDatabase<Recipe> database, List<IBotCommand> commands)
        {
            Database = database;
            Commands = commands;
        }

        public string HandleCommand(string message)
        {
            var query = message.Split(' ');
            foreach (var command in Commands)
            {
                if (command.Name == query[0])
                {
                    var result = command.Execute(Database, query.Skip(1).ToArray());
                    if (result != null)
                        return result;
                }
            }
            return HandleCommand("/help");
        }
    }
}
