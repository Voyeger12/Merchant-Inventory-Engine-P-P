using Microsoft.VisualStudio.TestTools.UnitTesting;
using MerchantInventoryEngine.Services;

namespace MerchantInventoryEngine.Tests
{
    [TestClass]
    public class PriceCalculatorTests
    {
        [TestMethod]
        public void CalculateFinalPrice_WithVariousMultipliers_ReturnsCorrectValue()
        {
            // Arrange
            var calculator = new PriceCalculator();
            decimal basePrice = 10.0m;
            decimal pMult = 1.2m;
            decimal lMult = 0.8m;
            decimal polMult = 1.5m;

            // 10 * 1.2 * 0.8 * 1.5 = 14.4
            decimal expected = 14.4m;

            // Act
            decimal result = calculator.CalculateFinalPrice(basePrice, pMult, lMult, polMult);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CalculateFinalPrice_WithZeroBasePrice_ReturnsZero()
        {
            // Arrange
            var calculator = new PriceCalculator();
            decimal basePrice = 0.0m;
            
            // Act
            decimal result = calculator.CalculateFinalPrice(basePrice, 1.2m, 1.5m, 1.0m);

            // Assert
            Assert.AreEqual(0.0m, result);
        }
    }
}
