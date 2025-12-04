using System;
using System.Collections.Generic;
using System.Text;

namespace Mozaika.CalculationModule
{
    
    public interface IRecipeComponent
    {
        decimal QuantityRequired { get; }
        decimal CostPerUnit { get; }
    }
}
