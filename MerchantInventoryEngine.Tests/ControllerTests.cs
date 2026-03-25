using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MerchantInventoryEngine.Controllers;
using MerchantInventoryEngine.Data;
using MerchantInventoryEngine.Models;
using MerchantInventoryEngine.Services;

namespace MerchantInventoryEngine.Tests
{
    [TestClass]
    [DoNotParallelize]
    public class ControllerTests
    {
        private string _testDbPath = string.Empty;
        private string _testExportPath = string.Empty;

        [TestInitialize]
        public void Setup()
        {
            _testDbPath = Path.Combine(Path.GetTempPath(), $"controller_test_inventory_{Guid.NewGuid():N}.db");
            _testExportPath = Path.Combine(Path.GetTempPath(), $"controller_export_test_{Guid.NewGuid():N}.csv");

            if (File.Exists(_testDbPath)) File.Delete(_testDbPath);
            if (File.Exists(_testExportPath)) File.Delete(_testExportPath);
        }

        [TestMethod]
        public void CalculateInventory_ReturnsCorrectCalculatedList()
        {
            // Arrange
            var dbHelper = new DatabaseHelper($"Data Source={_testDbPath}");
            var calculator = new PriceCalculator();
            var controller = new MerchantController(dbHelper, calculator);

            decimal pMult = 1.0m;
            decimal lMult = 2.0m;
            decimal polMult = 1.0m;

            // Act
            var inventory = controller.CalculateInventory(pMult, lMult, polMult);

            // Assert
            Assert.IsNotNull(inventory);
            Assert.IsNotEmpty(inventory, "Inventory should contain seeded items.");

            var potion = inventory.FirstOrDefault(i => i.ItemName == "Health Potion");
            Assert.IsNotNull(potion);
            
            // BasePrice is 10.0 in Seed, so 10.0 * 1.0 * 2.0 * 1.0 = 20.0
            Assert.AreEqual(20.0m, potion.FinalPrice);
            Assert.AreEqual("Potions", potion.CategoryName);
        }

        [TestMethod]
        public void ExportToCsv_WritesExpectedHeaderAndRows()
        {
            var dbHelper = new DatabaseHelper($"Data Source={_testDbPath}");
            var calculator = new PriceCalculator();
            var controller = new MerchantController(dbHelper, calculator);

            var rows = new List<InventoryItem>
            {
                new InventoryItem { Id = 1, ItemName = "Torch", CategoryName = "General Goods", BasePrice = 1.0m, FinalPrice = 1.2m },
                new InventoryItem { Id = 2, ItemName = "Dagger", CategoryName = "Weapons", BasePrice = 8.0m, FinalPrice = 9.6m }
            };

            controller.ExportToCsv(_testExportPath, rows);

            Assert.IsTrue(File.Exists(_testExportPath), "CSV export file was not created.");
            var content = File.ReadAllText(_testExportPath);

            StringAssert.Contains(content, "Id;ItemName;CategoryName;BasePrice;FinalPrice");
            StringAssert.Contains(content, "1;Torch;General Goods;1.0;1.2");
            StringAssert.Contains(content, "2;Dagger;Weapons;8.0;9.6");
        }

        [TestMethod]
        public void ExportToCsv_EscapesFieldsWithSpecialCharacters()
        {
            var dbHelper = new DatabaseHelper($"Data Source={_testDbPath}");
            var controller = new MerchantController(dbHelper, new PriceCalculator());

            var rows = new List<InventoryItem>
            {
                new InventoryItem { Id = 3, ItemName = "Scroll; of \"Fire\"", CategoryName = "Magic\nArtifacts", BasePrice = 10m, FinalPrice = 12m }
            };

            controller.ExportToCsv(_testExportPath, rows, includeBom: false);
            var content = File.ReadAllText(_testExportPath);

            StringAssert.Contains(content, "\"Scroll; of \"\"Fire\"\"\";");
            StringAssert.Contains(content, "\"Magic");
        }

        [TestMethod]
        public async Task CalculateInventoryAsync_WithOutOfRangeMultiplier_Throws()
        {
            var dbHelper = new DatabaseHelper($"Data Source={_testDbPath}");
            var controller = new MerchantController(dbHelper, new PriceCalculator());

            var thrown = false;
            try
            {
                await controller.CalculateInventoryAsync(0.01m, 1.0m, 1.0m);
            }
            catch (ArgumentOutOfRangeException)
            {
                thrown = true;
            }

            Assert.IsTrue(thrown, "Expected ArgumentOutOfRangeException for invalid multiplier.");
        }

        [TestMethod]
        public void CalculateInventory_AllowsBoundaryMultipliers()
        {
            var dbHelper = new DatabaseHelper($"Data Source={_testDbPath}");
            var controller = new MerchantController(dbHelper, new PriceCalculator());

            var low = controller.CalculateInventory(0.1m, 1.0m, 1.0m);
            var high = controller.CalculateInventory(5.0m, 1.0m, 1.0m);

            Assert.IsNotEmpty(low);
            Assert.IsNotEmpty(high);
        }

        [TestMethod]
        public void CalculateInventory_WithFactionMultiplier_ChangesPrice()
        {
            var dbHelper = new DatabaseHelper($"Data Source={_testDbPath}");
            var controller = new MerchantController(dbHelper, new PriceCalculator());

            var neutral = controller.CalculateInventory(1.0m, 1.0m, 1.0m, 1.0m);
            var enemy = controller.CalculateInventory(1.0m, 1.0m, 1.0m, 1.5m);

            var neutralPotion = neutral.First(i => i.ItemName == "Health Potion");
            var enemyPotion = enemy.First(i => i.ItemName == "Health Potion");

            Assert.IsGreaterThan(neutralPotion.FinalPrice, enemyPotion.FinalPrice);
        }

        [TestCleanup]
        public void Cleanup()
        {
            SqliteConnection.ClearAllPools();

            TryDeleteFile(_testDbPath);
            TryDeleteFile(_testExportPath);
        }

        private static void TryDeleteFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                return;
            }

            for (var i = 0; i < 5; i++)
            {
                try
                {
                    File.Delete(path);
                    return;
                }
                catch (IOException)
                {
                    System.Threading.Thread.Sleep(50);
                }
            }
        }
    }
}
