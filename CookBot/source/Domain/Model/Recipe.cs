using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace source.Domain.Model
{
    [Serializable]
    public class Recipe
    {
        public int Id { get; private set; }
        public string Name { get; }
        public string Description { get; }
        private Dictionary<IIngredient, IIngredientAmount> components;

        public ImmutableDictionary<IIngredient, IIngredientAmount> Components
        {
            get { return components.ToImmutableDictionary(); }
        }

        public Recipe()
        {
            components = new Dictionary<IIngredient, IIngredientAmount>();
        }

        public Recipe(string name, string description, Dictionary<IIngredient, IIngredientAmount> components)
        {
            Name = name;
            Description = description;
            this.components = components;
        }

        public string GetPrintableView()
        {
            string result = $"*Название: {Name}\n*Рецепт: {Description}\n*Ингридиенты:\n";
            return Components.Aggregate(result, (current, component) => current + $"-{component.Key.Name} " +
                                                                        $"{component.Value.Count} {component.Value.MeasureUnit}\n");
        }
    }
}
