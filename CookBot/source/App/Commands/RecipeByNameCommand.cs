using System;
using source.Domain;
using source.Domain.Model;
using source.Infrastructure.Databases;

namespace source.App.Commands
{
    class RecipeByNameCommand : IBotCommand
    {
        public string Name => "/recipeName";
        public string Description => "найти рецепт по названию";

        public string Execute(IDatabase<Recipe> db, params string[] arguments)
        {
            var recipeName = string.Join(" ", arguments);
            try
            {
                return db.GetAnySuitable(x => x.Name == recipeName).GetPrintableView();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
}
