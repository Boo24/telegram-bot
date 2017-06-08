using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using source.App.Commands;
using source.Domain.Model;
using source.Infrastructure.Databases;

namespace source.App.Commands
{
    public class HelpCommand : IBotCommand
    {
        public string Name => "/help";
        public string Description => "информация о поддерживаемых командах";
        private readonly Lazy<List<IBotCommand>> commands;
        private string helloMessage;

        public HelpCommand(Lazy<List<IBotCommand>> commands, [Named("HelloMessage")]string helloMess)
        {
            helloMessage = helloMess;
            this.commands = commands;
        }
        public string Execute(IDatabase<Recipe> db, params string[] arguments)
        {
            var result = helloMessage + "\n";
            result += string.Join("\n", commands.Value.Select(command => $"{command.Name} - {command.Description}."));
            return result;
        }
    }
}
