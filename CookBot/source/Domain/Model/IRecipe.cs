using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace source.Domain.Model
{
    public interface IRecipe
    {
        int Id { get; }
        string Name { get; }
        ImmutableDictionary<IIngredient, IIngredientAmount> Components { get; }
        string GetPrintableView();
    }
}
