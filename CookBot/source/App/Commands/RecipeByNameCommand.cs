using System;
using source.Domain;
using source.Domain.Model;
using source.Infrastructure.Databases;

namespace source.App.Commands
{
    public class RecipeByNameCommand : IBotCommand
    {
        public string Name => "/recipeName";
        public string Description => "найти рецепт по названию";

        public string Execute(IDatabase<Recipe> db, params string[] arguments)
        {
            var recipeName = string.Join(" ", arguments);
            try
            {
                return db.GetAnySuitable(x => string.Equals(x.Name, recipeName, StringComparison.CurrentCultureIgnoreCase)).GetPrintableView();
            }
            catch (InvalidOperationException)
            {
                return "Подходящий рецепт не найден :(";
            }
        }
    }
}
