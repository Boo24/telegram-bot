using System.Linq;
using source.Infrastructure.Databases;
using source.Domain.Model;

namespace source.App.Commands
{
    public class RecipeByIngredientsCommand : IBotCommand
    {
        public string Description => "получить список рецептов, в которых содержится указанный ингридиент";
        public string Name => "/ingr";

        private IDatabase<IRecipe> Database { get; }

        public RecipeByIngredientsCommand(IDatabase<IRecipe> database)
        {
            Database = database;
        }
        public BotCommandResult Execute(string[] arguments)
        {
            var suitableRecipes = Database.GetAllSuitable(x => arguments
                        .All(z => x.Components.Keys.Select(y => y.Name.ToLower()).Contains(z.ToLower())));

            if (!suitableRecipes.Any())
                return new BotCommandResult(BotCode.Bad, "К сожалению, ничего подходящего не найдено: (");

            return new BotCommandResult(
                BotCode.Good,
                string.Join("\n", suitableRecipes.Select(res => res.GetPrintableView())));
        }
    }
}