using System;
using System.Collections.Generic;

namespace CookBot.Domain.Model
{
    [Serializable]
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Dictionary<IIngredient, IIngredientAmount> Components = new Dictionary<IIngredient, IIngredientAmount>();

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
