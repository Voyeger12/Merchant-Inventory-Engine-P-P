using System.IO;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MerchantInventoryEngine.Data;

namespace MerchantInventoryEngine.Tests
{
    [TestClass]
    public class DatabaseTests
    {
        private string _testDbPath = string.Empty;

        [TestInitialize]
        public void Setup()
        {
            _testDbPath = Path.Combine(Path.GetTempPath(), $"test_inventory_{Guid.NewGuid():N}.db");
            if (File.Exists(_testDbPath)) File.Delete(_testDbPath);
        }

        [TestMethod]
        public void DatabaseHelper_CreatesDatabase_AndSeedsData()
        {
            // Act: Konstruktor ruft InitializeDatabase und SeedDatabase auf
            var dbHelper = new DatabaseHelper($"Data Source={_testDbPath}");

            // Assert: Überprüfe, ob Datei existiert
            Assert.IsTrue(File.Exists(_testDbPath), "Die Datenbankdatei wurde nicht erstellt.");

            // Assert: Überprüfe, ob Daten geladen werden können
            var categories = dbHelper.GetCategories();
            var items = dbHelper.GetItems();
            var personalities = dbHelper.GetModifiersByType("Personality");
            var locations = dbHelper.GetModifiersByType("Location");
            var political = dbHelper.GetModifiersByType("Political");

            Assert.IsGreaterThanOrEqualTo(6, categories.Count, "Expected expanded category seed data.");
            Assert.IsGreaterThanOrEqualTo(20, items.Count, "Expected expanded item seed data.");
            Assert.IsGreaterThanOrEqualTo(6, personalities.Count, "Expected expanded personality modifiers.");
            Assert.IsGreaterThanOrEqualTo(4, locations.Count, "Expected location modifiers.");
            Assert.IsGreaterThanOrEqualTo(4, political.Count, "Expected political modifiers.");
            
            // Spezifische korrekte Zuordnung prüfen
            var ironSword = items.FirstOrDefault(i => i.Name == "Iron Sword");
            Assert.IsNotNull(ironSword, "Iron Sword sollte in der generierten DB existieren.");
            Assert.AreEqual(15.0m, ironSword.BasePrice);
            Assert.IsTrue(dbHelper.IsDatabaseHealthy(), "Healthy database should pass integrity checks.");
        }

        [TestMethod]
        [DoNotParallelize]
        public void DatabaseHelper_HealthCheck_ReturnsFalse_OnForeignKeyCorruption()
        {
            var dbHelper = new DatabaseHelper($"Data Source={_testDbPath}");

            using var connection = new SqliteConnection($"Data Source={_testDbPath}");
            connection.Open();

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "PRAGMA foreign_keys = OFF;";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO Items (Name, BasePrice, CategoryId) VALUES ('Broken Item', 1.0, 999999);";
                cmd.ExecuteNonQuery();
            }

            Assert.IsFalse(dbHelper.IsDatabaseHealthy(), "Database with invalid FK references should be unhealthy.");
        }

        [TestCleanup]
        public void Cleanup()
        {
            SqliteConnection.ClearAllPools();
            if (File.Exists(_testDbPath)) File.Delete(_testDbPath);
        }
    }
}
