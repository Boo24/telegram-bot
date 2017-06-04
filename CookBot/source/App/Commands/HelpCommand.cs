using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookBot.Domain.Model;
using CookBot.Infrastructure.Databases;

namespace source.App.Commands
{
    class HelpCommand : IBotCommand
    {
        public string Name => "/help";
        public string Description => "отобразить это сообщение";

        private readonly List<IBotCommand> commands;

        public HelpCommand(List<IBotCommand> commands)
        {
            this.commands = commands;
        }

        public string Execute(IDatabase<Recipe> db, params string[] arguments)
        {
            var result = "Привет! Меня зовут Cook Bot :D. У меня самые лучшие рецепты. Вот список команд, которые я могу выполнить: \n";
            result += string.Join("\n", commands.Select(command => $"{command.Name} - {command.Description}."));
            return result;
        }
    }
}
