using System;
using source.Domain;
using source.Domain.Model;
using source.Infrastructure.Databases;

namespace source.App.Commands
{
    public class RecipeByNameCommand : IBotCommand
    {
        public string Name => "/recipe";
        public string Description => "найти рецепт по названию";
        public string Result { get; private set; }

        public BotCommandResult Execute(IDatabase<Recipe> db, string[] arguments)
        {
            var recipeName = string.Join(" ", arguments);
            try
            {
                Result = db
                    .GetAnySuitable(x => string.Equals(x.Name, recipeName, StringComparison.CurrentCultureIgnoreCase))
                    .GetPrintableView();
                return BotCommandResult.Good;
            }
            catch (InvalidOperationException)
            {
                return BotCommandResult.Bad;
            }
        }
    }
}
