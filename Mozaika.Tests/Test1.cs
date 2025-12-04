using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mozaika.CalculationModule;
using System;
using System.Collections.Generic;

namespace Mozaika.Tests
{
    // Заглушка для теста
    public class TestRecipeComponent : IRecipeComponent
    {
        public int MaterialId { get; set; }
        public decimal QuantityRequired { get; set; }
        public decimal CostPerUnit { get; set; }
    }

    [TestClass]
    public class CostCalculatorTests
    {
        private CostCalculator _calculator;

        [TestInitialize]
        public void Setup()
        {
            _calculator = new CostCalculator();
        }

        [TestMethod]
        public void CalculateTotalMaterialCost_NullInput_ReturnsZero()
        {
            List<IRecipeComponent> components = null;
            decimal result = _calculator.CalculateTotalMaterialCost(components);
            Assert.AreEqual(0, result, "ОШИБКА: При передаче NULL метод должен возвращать 0.");
        }

        [TestMethod]
        public void CalculateTotalMaterialCost_NegativeQuantity_ThrowsException()
        {
            var components = new List<IRecipeComponent>
            {
                new TestRecipeComponent { QuantityRequired = -5, CostPerUnit = 10 }
            };

            // Этот тест ПРОВАЛИТСЯ, если в калькуляторе отключена проверка
            var exception = Assert.ThrowsException<ArgumentException>(() =>
            {
                _calculator.CalculateTotalMaterialCost(components);
            }, "ОШИБКА: Исключение не было выброшено!");
        }
    }
}