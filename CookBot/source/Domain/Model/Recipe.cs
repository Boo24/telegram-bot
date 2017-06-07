using System;
using System.Collections.Immutable;
using System.Collections.Generic;

namespace source.Domain.Model
{
    [Serializable]
    public class Recipe
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }

        private Dictionary<IIngredient, IIngredientAmount> components;
        public ImmutableDictionary<IIngredient, IIngredientAmount> Components { get { return components.ToImmutableDictionary(); } }

        public Recipe()
        {
            components = new Dictionary<IIngredient, IIngredientAmount>();
        }
    }
}
