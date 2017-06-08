using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using source.Domain.Model;
using source.Infrastructure.Databases;

namespace source.App.Commands
{
    public class RecipeListCommand : IBotCommand
    {
        public string Name => "/recipeList";

        public string Description => "отобразить список всех рецептов";

        public string Execute(IDatabase<Recipe> db, params string[] arguments)
        {
            var recipesNames = db.GetAllSuitable(_ => true).Select(recipe => recipe.Name).ToArray();
            var result = new StringBuilder();
            for (var i = 0; i < recipesNames.Length; i++)
            {
                result.Append((i + 1) + ". " + recipesNames[i] + "\n");
            }
            return result.ToString();
        }
    }
}
