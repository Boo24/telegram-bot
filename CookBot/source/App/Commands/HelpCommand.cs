using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
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

        public BotCommandResult Execute(IDatabase<Recipe> db, string[] arguments)
        {
            var result = helloMessage + "\n";
            result += string.Join("\n", commands.Value.Select(command => $"{command.Name} - {command.Description}."));
            return new BotCommandResult(BotCode.Good, result);
        }
    }
}
