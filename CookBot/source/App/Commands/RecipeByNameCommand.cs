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

        private IDatabase<IRecipe> Database { get; }

        public RecipeByNameCommand(IDatabase<IRecipe> database)
        {
            Database = database;
        }

        public BotCommandResult Execute(string[] arguments)
        {
            var recipeName = string.Join(" ", arguments);
            try
            {
                var result = Database
                    .GetAnySuitable(x => string.Equals(x.Name, recipeName, StringComparison.CurrentCultureIgnoreCase))
                    .GetPrintableView();
                return new BotCommandResult(BotCode.Good, result);
            }
            catch (InvalidOperationException)
            {
                return new BotCommandResult(BotCode.Bad, "К сожалению, ничего подходящего не найдено :(");
            }
        }
    }
}
