using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using MerchantInventoryEngine.Models;

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
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            
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
            
            SeedDatabase(connection);
        }

        private void SeedDatabase(SqliteConnection connection)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM Categories";
            var count = Convert.ToInt32(cmd.ExecuteScalar());

            if (count == 0)
            {
                cmd.CommandText = @"
                    INSERT INTO Categories (Name) VALUES ('Weapons'), ('Armor'), ('Potions'), ('General Goods');
                    
                    INSERT INTO Items (Name, BasePrice, CategoryId) VALUES 
                    ('Iron Sword', 15.0, 1),
                    ('Steel Sword', 30.0, 1),
                    ('Leather Armor', 25.0, 2),
                    ('Health Potion', 10.0, 3),
                    ('Rope', 2.0, 4);

                    INSERT INTO Modifiers (Name, Multiplier, Type) VALUES 
                    ('Friendly', 0.9, 'Personality'),
                    ('Greedy', 1.5, 'Personality'),
                    ('City', 1.0, 'Location'),
                    ('Remote Village', 1.2, 'Location'),
                    ('Peace', 1.0, 'Political'),
                    ('War', 2.0, 'Political');
                ";
                cmd.ExecuteNonQuery();
            }
        }

        public List<Category> GetCategories()
        {
            var categories = new List<Category>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

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

        public List<Item> GetItems()
        {
            var items = new List<Item>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

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

        public List<Modifier> GetModifiersByType(string type)
        {
            var modifiers = new List<Modifier>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

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
    }
}