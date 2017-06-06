using System;
using System.Collections.Generic;

namespace CookBot.Domain.Model
{
    [Serializable]
    public class Recipe
    {
        public int Id { get;}
        public string Name { get;}
        public string Description { get; }

        public Dictionary<IIngredient, IIngredientAmount> Components;
    }
}
