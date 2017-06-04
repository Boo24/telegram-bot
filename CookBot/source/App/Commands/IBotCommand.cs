using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookBot.Domain.Model;
using CookBot.Infrastructure.Databases;

namespace source.App.Commands
{
    enum BotCode
    {
        Good,
        BadCommand,
        BadArguments
    }

    interface IBotCommand
    {
        string Name { get; }
        string Description { get; }
        string Execute(IDatabase<Recipe> db, params string[] arguments);
    }
}
