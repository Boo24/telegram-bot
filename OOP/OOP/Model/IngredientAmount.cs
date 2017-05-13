using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP.Model
{
    class IngredientAmount: IIngredientAmount


    {
    public string MeasureUnit { get; private set; }
    public double Count { get; private set; }


    public void Change(double delta)
    {
        Count += delta;
    }


    }
}
