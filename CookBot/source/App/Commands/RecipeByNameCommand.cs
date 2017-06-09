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

        public string Execute(IDatabase<Recipe> db, string[] arguments)
        {
            var recipeName = string.Join(" ", arguments);
            try
            {
                var result = db
                    .GetAnySuitable(x => string.Equals(x.Name, recipeName, StringComparison.CurrentCultureIgnoreCase))
                    .GetPrintableView();
                return result;
            }
            catch (InvalidOperationException)
            {
                return "Нет подходящего рецепта";
            }
        }
    }
}
