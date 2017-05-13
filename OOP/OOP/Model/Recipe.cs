using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP.Model
{
    class Recipe
    {
        public readonly string Name;
        public string Description { get; private set; }
        private Dictionary<IIngredient, IIngredientAmount> Components;

        public Dictionary<IIngredient, IIngredientAmount> GetComponents()
        {
            throw new NotImplementedException();
        }

        void AddIngredient(IIngredient ingr, IIngredientAmount amount)
        {
            
        }

        void DelIngredient(IIngredient ingr)
        {
            
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
