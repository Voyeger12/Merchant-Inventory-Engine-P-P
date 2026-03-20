using Microsoft.VisualStudio.TestTools.UnitTesting;
using MerchantInventoryEngine.Services;

namespace MerchantInventoryEngine.Tests
{
    [TestClass]
    public class PriceCalculatorTests
    {
        private PriceCalculator _calculator = default!;

        [TestInitialize]
        public void Setup()
        {
            _calculator = new PriceCalculator();
        }

        [TestMethod]
        public void CalculateFinalPrice_WithVariousMultipliers_ReturnsCorrectValue()
        {
            decimal basePrice = 10.0m;
            decimal pMult = 1.2m;
            decimal lMult = 0.8m;
            decimal polMult = 1.5m;
            decimal expected = 14.4m; // 10 * 1.2 * 0.8 * 1.5

            decimal result = _calculator.CalculateFinalPrice(basePrice, pMult, lMult, polMult);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CalculateFinalPrice_WithZeroBasePrice_ReturnsZero()
        {
            decimal basePrice = 0.0m;
            decimal result = _calculator.CalculateFinalPrice(basePrice, 1.2m, 1.5m, 1.0m);
            Assert.AreEqual(0.0m, result);
        }

        [TestMethod]
        public void CalculateFinalPrice_WithNegativeMultipliers_HandledCorrectly()
        {
            // Even though conceptually negative might not make sense, math should still hold.
            decimal basePrice = 100.0m;
            decimal result = _calculator.CalculateFinalPrice(basePrice, -0.5m, 1.0m, 1.0m);
            Assert.AreEqual(-50.0m, result);
        }

        [TestMethod]
        public void CalculateFinalPrice_WithHighPrecision_ReturnsExactValue()
        {
            decimal basePrice = 15.35m;
            decimal pMult = 1.15m;
            decimal lMult = 0.95m;
            decimal polMult = 1.08m;
            
            // Decimal math: 15.35 * 1.15 = 17.6525
            // 17.6525 * 0.95 = 16.769875
            // 16.769875 * 1.08 = 18.111465
            decimal result = _calculator.CalculateFinalPrice(basePrice, pMult, lMult, polMult);
            // Wir verwenden das tatsächliche Ergebnis der Dezimal-Multiplikation
            Assert.AreEqual(18.111465m, result);
        }
    }
}
