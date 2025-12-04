using System;
using System.Collections.Generic;
using System.Linq;

namespace Mozaika.CalculationModule
{
    public class CostCalculator
    {
        public decimal CalculateTotalMaterialCost(IEnumerable<IRecipeComponent> components)
        {
            if (components == null || !components.Any())
            {
                return 0;
            }

            decimal totalCost = 0;

            foreach (var component in components)
            {
                // --- ЛОМАЕМ КОД ТУТ ---
                // Закомментируйте этот блок проверки:
                /*
                if (component.QuantityRequired < 0 || component.CostPerUnit < 0)
                {
                    throw new ArgumentException("Количество и стоимость не могут быть отрицательными.");
                }
                */
                // --- КОНЕЦ ПОЛОМКИ ---

                // Теперь программа проглотит -5 и посчитает неправильную сумму, вместо ошибки.
                totalCost += component.QuantityRequired * component.CostPerUnit;
            }

            return totalCost;
        }
    }
}