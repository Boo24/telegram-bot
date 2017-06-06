using System;
using System.Collections.Generic;

namespace source.Domain.Model
{
    [Serializable]
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Dictionary<IIngredient, IIngredientAmount> Components { get; set; }

        public Recipe()
        {
            Components = new Dictionary<IIngredient, IIngredientAmount>();
        }
    }
}
