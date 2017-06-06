using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using source.Infrastructure.Databases;
using source.Domain.Model;

namespace source.App.Commands
{
    class RecipeByIngredientsCommand : IBotCommand
    {
        public string Description => "получить список рецептов, в которых содержится указанный ингридиент";

        public string Name => "/recipeIngredients";

        public string Execute(IDatabase<Recipe> db, params string[] arguments)
        {
            throw new NotImplementedException();
        }
    }
}
