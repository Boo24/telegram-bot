using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using source.Domain;
using source.Infrastructure.Databases;
using source.Domain.Model;

namespace source.App.Commands
{
    class RecipeByIngredientsCommand : IBotCommand
    {
        public string Description => "получить список рецептов, в которых содержится указанный ингридиент";

        public string Name => "/recipeIngr";

        public string Execute(IDatabase<Recipe> db, params string[] arguments)
        {
            var suitableRecipes = db.GetAllSuitable(x => arguments
                        .All(z => x.Components.Keys.Select(y => y.Name.ToLower()).Contains(z.ToLower())));

            if (!suitableRecipes.Any())
                return "Нет подходящих рецептов :(";

            return String.Join("\n", suitableRecipes.Select(res => res.GetPrintableView()));
        }
    }
}