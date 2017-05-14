using System.Collections.Generic;
using CookBot.Domain.Model;

namespace CookBot.Domain.Databases
{
    public interface IDatabase
    {
        Recipe GetRecipeByName(string name);
        IEnumerable<Recipe> GetRecipesByIngredient(IIngredient ingredient);
    }
}
