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


        public string GetPrintableView()
        {
            string result = $"*Название: {Name}\n*Рецепт: {Description}\n*Ингридиенты:\n";
            foreach (var component in Components)
                result += $"-{component.Key.Name} {component.Value.Count} {component.Value.MeasureUnit}\n";
            return result;
        }
    }
}
