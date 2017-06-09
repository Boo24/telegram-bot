using System.Linq;
using source.Infrastructure.Databases;
using source.Domain.Model;

namespace source.App.Commands
{
    public class RecipeByIngredientsCommand : IBotCommand
    {
        public string Description => "получить список рецептов, в которых содержится указанный ингридиент";
        public string Name => "/ingr";

        public string Execute(IDatabase<Recipe> db, string[] arguments)
        {
            var suitableRecipes = db.GetAllSuitable(x => arguments
                        .All(z => x.Components.Keys.Select(y => y.Name.ToLower()).Contains(z.ToLower())));

            if (!suitableRecipes.Any())
                return "Нет подходящего рецепты";

            return string.Join("\n", suitableRecipes.Select(res => res.GetPrintableView()));
        }
    }
}