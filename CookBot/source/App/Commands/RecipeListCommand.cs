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
        public string Name => "/all";
        public string Description => "отобразить список всех рецептов";
        IDatabase<IRecipe> Database { get; }

        public RecipeListCommand(IDatabase<IRecipe> database)
        {
            Database = database;
        }

        public BotCommandResult Execute(string[] arguments)
        {
            var recipesNames = Database.GetAllSuitable(_ => true).Select(recipe => recipe.Name).ToArray();
            var result = new StringBuilder();
            for (var i = 0; i < recipesNames.Length; i++)
            {
                result.Append((i + 1) + ". " + recipesNames[i] + "\n");
            }
            return new BotCommandResult(BotCode.Good, result.ToString());
        }
    }
}
