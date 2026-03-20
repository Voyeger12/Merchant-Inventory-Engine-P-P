using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using MerchantInventoryEngine.Models;
using MerchantInventoryEngine.Services;

namespace MerchantInventoryEngine.Data
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(string connectionString = "Data Source=merchant_inventory.db")
        {
            _connectionString = connectionString;
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                EnableForeignKeys(connection);
                EnsureSchemaVersionTable(connection);
                ApplyMigrations(connection);
                SeedDatabase(connection);
                AppLogger.Info("Database initialized successfully.");
            }
            catch (Exception ex)
            {
                AppLogger.Error("Database initialization failed.", ex);
                throw;
            }
        }

        private static void EnableForeignKeys(SqliteConnection connection)
        {
            using var pragmaCommand = connection.CreateCommand();
            pragmaCommand.CommandText = "PRAGMA foreign_keys = ON;";
            pragmaCommand.ExecuteNonQuery();
        }

        private static void EnsureSchemaVersionTable(SqliteConnection connection)
        {
            using var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS SchemaVersion (
                    Id INTEGER PRIMARY KEY CHECK (Id = 1),
                    Version INTEGER NOT NULL
                );

                INSERT OR IGNORE INTO SchemaVersion (Id, Version) VALUES (1, 0);
            ";
            command.ExecuteNonQuery();
        }

        private static int GetSchemaVersion(SqliteConnection connection)
        {
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT Version FROM SchemaVersion WHERE Id = 1";
            return Convert.ToInt32(command.ExecuteScalar());
        }

        private static void SetSchemaVersion(SqliteConnection connection, int version)
        {
            using var command = connection.CreateCommand();
            command.CommandText = "UPDATE SchemaVersion SET Version = $version WHERE Id = 1";
            command.Parameters.AddWithValue("$version", version);
            command.ExecuteNonQuery();
        }

        private void ApplyMigrations(SqliteConnection connection)
        {
            var currentVersion = GetSchemaVersion(connection);

            if (currentVersion < 1)
            {
                using var command = connection.CreateCommand();
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Categories (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL
                    );

                    CREATE TABLE IF NOT EXISTS Items (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        BasePrice DECIMAL NOT NULL,
                        CategoryId INTEGER NOT NULL,
                        FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
                    );

                    CREATE TABLE IF NOT EXISTS Modifiers (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Multiplier DECIMAL NOT NULL,
                        Type TEXT NOT NULL
                    );
                ";
                command.ExecuteNonQuery();
                SetSchemaVersion(connection, 1);
                AppLogger.Info("Applied DB migration: v1");
                currentVersion = 1;
            }

            if (currentVersion < 2)
            {
                using var command = connection.CreateCommand();
                command.CommandText = @"
                    CREATE UNIQUE INDEX IF NOT EXISTS UX_Categories_Name ON Categories(Name);
                    CREATE UNIQUE INDEX IF NOT EXISTS UX_Items_Name ON Items(Name);
                    CREATE UNIQUE INDEX IF NOT EXISTS UX_Modifiers_Name_Type ON Modifiers(Name, Type);
                ";
                command.ExecuteNonQuery();
                SetSchemaVersion(connection, 2);
                AppLogger.Info("Applied DB migration: v2");
            }
        }

        private void SeedDatabase(SqliteConnection connection)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT OR IGNORE INTO Categories (Name) VALUES
                ('Weapons'),
                ('Armor'),
                ('Potions'),
                ('General Goods'),
                ('Magic Artifacts'),
                ('Food & Supplies');

                INSERT OR IGNORE INTO Items (Name, BasePrice, CategoryId) VALUES 
                ('Iron Sword', 15.0, (SELECT Id FROM Categories WHERE Name = 'Weapons')),
                ('Steel Sword', 30.0, (SELECT Id FROM Categories WHERE Name = 'Weapons')),
                ('Dagger', 8.0, (SELECT Id FROM Categories WHERE Name = 'Weapons')),
                ('Longbow', 28.0, (SELECT Id FROM Categories WHERE Name = 'Weapons')),
                ('Crossbow', 45.0, (SELECT Id FROM Categories WHERE Name = 'Weapons')),
                ('Warhammer', 40.0, (SELECT Id FROM Categories WHERE Name = 'Weapons')),

                ('Leather Armor', 25.0, (SELECT Id FROM Categories WHERE Name = 'Armor')),
                ('Chainmail', 65.0, (SELECT Id FROM Categories WHERE Name = 'Armor')),
                ('Plate Armor', 120.0, (SELECT Id FROM Categories WHERE Name = 'Armor')),
                ('Wooden Shield', 18.0, (SELECT Id FROM Categories WHERE Name = 'Armor')),
                ('Tower Shield', 55.0, (SELECT Id FROM Categories WHERE Name = 'Armor')),

                ('Health Potion', 10.0, (SELECT Id FROM Categories WHERE Name = 'Potions')),
                ('Mana Potion', 14.0, (SELECT Id FROM Categories WHERE Name = 'Potions')),
                ('Stamina Potion', 9.0, (SELECT Id FROM Categories WHERE Name = 'Potions')),
                ('Antidote', 7.0, (SELECT Id FROM Categories WHERE Name = 'Potions')),

                ('Rope', 2.0, (SELECT Id FROM Categories WHERE Name = 'General Goods')),
                ('Torch', 1.0, (SELECT Id FROM Categories WHERE Name = 'General Goods')),
                ('Lantern', 6.0, (SELECT Id FROM Categories WHERE Name = 'General Goods')),
                ('Traveler Backpack', 12.0, (SELECT Id FROM Categories WHERE Name = 'General Goods')),
                ('Lockpick Set', 20.0, (SELECT Id FROM Categories WHERE Name = 'General Goods')),

                ('Ring of Protection', 250.0, (SELECT Id FROM Categories WHERE Name = 'Magic Artifacts')),
                ('Wand of Sparks', 130.0, (SELECT Id FROM Categories WHERE Name = 'Magic Artifacts')),
                ('Enchanted Cloak', 180.0, (SELECT Id FROM Categories WHERE Name = 'Magic Artifacts')),

                ('Dried Meat', 3.0, (SELECT Id FROM Categories WHERE Name = 'Food & Supplies')),
                ('Bread Loaf', 1.0, (SELECT Id FROM Categories WHERE Name = 'Food & Supplies')),
                ('Waterskin', 4.0, (SELECT Id FROM Categories WHERE Name = 'Food & Supplies')),
                ('Spice Pack', 9.0, (SELECT Id FROM Categories WHERE Name = 'Food & Supplies'));

                INSERT OR IGNORE INTO Modifiers (Name, Multiplier, Type) VALUES 
                ('Friendly', 0.90, 'Personality'),
                ('Honorable', 0.95, 'Personality'),
                ('Neutral', 1.00, 'Personality'),
                ('Shrewd', 1.15, 'Personality'),
                ('Greedy', 1.50, 'Personality'),
                ('Black Market Dealer', 1.75, 'Personality'),
                ('Desperate', 0.80, 'Personality'),
                ('Noble Quarter Merchant', 1.30, 'Personality'),

                ('Capital City', 1.10, 'Location'),
                ('City', 1.00, 'Location'),
                ('Harbor District', 1.05, 'Location'),
                ('Remote Village', 1.20, 'Location'),
                ('Mountain Keep', 1.35, 'Location'),
                ('Border Outpost', 1.25, 'Location'),

                ('Peace', 1.00, 'Political'),
                ('Tension', 1.10, 'Political'),
                ('Trade Embargo', 1.40, 'Political'),
                ('War', 2.00, 'Political'),
                ('Royal Subsidy', 0.85, 'Political'),
                ('Rebellion', 1.60, 'Political');
            ";
            cmd.ExecuteNonQuery();
        }

        public List<Category> GetCategories()
        {
            var categories = new List<Category>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            EnableForeignKeys(connection);

            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Name FROM Categories";
            
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                categories.Add(new Category
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }

            return categories;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            var categories = new List<Category>();
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            EnableForeignKeys(connection);

            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Name FROM Categories";

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                categories.Add(new Category
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }

            return categories;
        }

        public List<Item> GetItems()
        {
            var items = new List<Item>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            EnableForeignKeys(connection);

            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Name, BasePrice, CategoryId FROM Items";
            
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                items.Add(new Item
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    BasePrice = reader.GetDecimal(2),
                    CategoryId = reader.GetInt32(3)
                });
            }

            return items;
        }

        public async Task<List<Item>> GetItemsAsync()
        {
            var items = new List<Item>();
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            EnableForeignKeys(connection);

            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Name, BasePrice, CategoryId FROM Items";

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                items.Add(new Item
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    BasePrice = reader.GetDecimal(2),
                    CategoryId = reader.GetInt32(3)
                });
            }

            return items;
        }

        public List<Modifier> GetModifiersByType(string type)
        {
            var modifiers = new List<Modifier>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            EnableForeignKeys(connection);

            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Name, Multiplier, Type FROM Modifiers WHERE Type = $type";
            command.Parameters.AddWithValue("$type", type);
            
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                modifiers.Add(new Modifier
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Multiplier = reader.GetDecimal(2),
                    Type = reader.GetString(3)
                });
            }

            return modifiers;
        }

        public async Task<List<Modifier>> GetModifiersByTypeAsync(string type)
        {
            var modifiers = new List<Modifier>();
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            EnableForeignKeys(connection);

            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Name, Multiplier, Type FROM Modifiers WHERE Type = $type";
            command.Parameters.AddWithValue("$type", type);

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                modifiers.Add(new Modifier
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Multiplier = reader.GetDecimal(2),
                    Type = reader.GetString(3)
                });
            }

            return modifiers;
        }

        public bool IsDatabaseHealthy()
        {
            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                EnableForeignKeys(connection);

                using var integrityCommand = connection.CreateCommand();
                integrityCommand.CommandText = "PRAGMA integrity_check;";
                var integrity = Convert.ToString(integrityCommand.ExecuteScalar());
                if (!string.Equals(integrity, "ok", StringComparison.OrdinalIgnoreCase))
                {
                    AppLogger.Error($"Integrity check failed: {integrity}");
                    return false;
                }

                using var fkCommand = connection.CreateCommand();
                fkCommand.CommandText = "PRAGMA foreign_key_check;";
                using var fkReader = fkCommand.ExecuteReader();
                var hasForeignKeyViolations = fkReader.Read();
                if (hasForeignKeyViolations)
                {
                    AppLogger.Error("Foreign key check failed.");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                AppLogger.Error("Health check failed.", ex);
                return false;
            }
        }
    }
}