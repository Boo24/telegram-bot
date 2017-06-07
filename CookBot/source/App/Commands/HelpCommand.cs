using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using source.Domain.Model;
using source.Infrastructure.Databases;

namespace source.App.Commands
{
    class HelpCommand : IBotCommand
    {
        public string Name => "/help";
        public string Description => "информация о поддерживаемых командах";
        private readonly Lazy<List<IBotCommand>> commands;

        public HelpCommand(Lazy<List<IBotCommand>> commands)
        {
            this.commands = commands;
        }
        public string Execute(IDatabase<Recipe> db, params string[] arguments)
        {
            var result = "Привет! Меня зовут Cook Bot :D. У меня самые лучшие рецепты. Вот список команд, которые я могу выполнить: \n";
            result += string.Join("\n", commands.Value.Select(command => $"{command.Name} - {command.Description}."));
            return result;
        }
    }
}
