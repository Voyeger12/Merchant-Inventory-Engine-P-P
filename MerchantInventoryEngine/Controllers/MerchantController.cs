using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerchantInventoryEngine.Data;
using MerchantInventoryEngine.Models;
using MerchantInventoryEngine.Services;

namespace MerchantInventoryEngine.Controllers
{
    public class MerchantController
    {
        private const decimal MinMultiplier = 0.1m;
        private const decimal MaxMultiplier = 5.0m;

        private readonly DatabaseHelper _db;
        private readonly PriceCalculator _calculator;

        public MerchantController(DatabaseHelper db, PriceCalculator calculator)
        {
            _db = db;
            _calculator = calculator;
        }

        public List<Modifier> GetModifiers(string type)
        {
            return _db.GetModifiersByType(type);
        }

        public Task<List<Modifier>> GetModifiersAsync(string type)
        {
            return _db.GetModifiersByTypeAsync(type);
        }

        public List<InventoryItem> CalculateInventory(decimal personalityMultiplier, decimal locationMultiplier, decimal politicalMultiplier)
        {
            return CalculateInventory(personalityMultiplier, locationMultiplier, politicalMultiplier, 1.0m);
        }

        public List<InventoryItem> CalculateInventory(decimal personalityMultiplier, decimal locationMultiplier, decimal politicalMultiplier, decimal factionMultiplier)
        {
            ValidateMultiplierRange(personalityMultiplier, nameof(personalityMultiplier));
            ValidateMultiplierRange(locationMultiplier, nameof(locationMultiplier));
            ValidateMultiplierRange(politicalMultiplier, nameof(politicalMultiplier));
            ValidateMultiplierRange(factionMultiplier, nameof(factionMultiplier));

            var items = _db.GetItems();
            var categories = _db.GetCategories().ToDictionary(c => c.Id, c => c.Name);
            var inventory = new List<InventoryItem>();

            foreach(var item in items)
            {
                var final = _calculator.CalculateFinalPrice(item.BasePrice, personalityMultiplier, locationMultiplier, politicalMultiplier * factionMultiplier);
                inventory.Add(new InventoryItem
                {
                    Id = item.Id,
                    ItemName = item.Name,
                    CategoryName = categories.ContainsKey(item.CategoryId) ? categories[item.CategoryId] : "Unknown",
                    BasePrice = item.BasePrice,
                    FinalPrice = final
                });
            }

            return inventory;
        }

        public Task<List<InventoryItem>> CalculateInventoryAsync(decimal personalityMultiplier, decimal locationMultiplier, decimal politicalMultiplier)
        {
            return CalculateInventoryAsync(personalityMultiplier, locationMultiplier, politicalMultiplier, 1.0m);
        }

        public async Task<List<InventoryItem>> CalculateInventoryAsync(decimal personalityMultiplier, decimal locationMultiplier, decimal politicalMultiplier, decimal factionMultiplier)
        {
            ValidateMultiplierRange(personalityMultiplier, nameof(personalityMultiplier));
            ValidateMultiplierRange(locationMultiplier, nameof(locationMultiplier));
            ValidateMultiplierRange(politicalMultiplier, nameof(politicalMultiplier));
            ValidateMultiplierRange(factionMultiplier, nameof(factionMultiplier));

            var items = await _db.GetItemsAsync();
            var categories = (await _db.GetCategoriesAsync()).ToDictionary(c => c.Id, c => c.Name);
            var inventory = new List<InventoryItem>();

            foreach (var item in items)
            {
                var final = _calculator.CalculateFinalPrice(item.BasePrice, personalityMultiplier, locationMultiplier, politicalMultiplier * factionMultiplier);
                inventory.Add(new InventoryItem
                {
                    Id = item.Id,
                    ItemName = item.Name,
                    CategoryName = categories.ContainsKey(item.CategoryId) ? categories[item.CategoryId] : "Unknown",
                    BasePrice = item.BasePrice,
                    FinalPrice = final
                });
            }

            return inventory;
        }

        public List<InventoryItem> FilterInventory(IEnumerable<InventoryItem> source, string? nameQuery, string? category, decimal? minFinalPrice, decimal? maxFinalPrice)
        {
            var query = source ?? Enumerable.Empty<InventoryItem>();

            if (!string.IsNullOrWhiteSpace(nameQuery))
            {
                query = query.Where(i => i.ItemName.Contains(nameQuery, System.StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(i => string.Equals(i.CategoryName, category, System.StringComparison.OrdinalIgnoreCase));
            }

            if (minFinalPrice.HasValue)
            {
                query = query.Where(i => i.FinalPrice >= minFinalPrice.Value);
            }

            if (maxFinalPrice.HasValue)
            {
                query = query.Where(i => i.FinalPrice <= maxFinalPrice.Value);
            }

            return query.ToList();
        }

        public void ExportToCsv(string filePath, List<InventoryItem> items, bool includeBom = true)
        {
            var content = BuildCsv(items);
            var encoding = new UTF8Encoding(includeBom);
            File.WriteAllText(filePath, content, encoding);
            AppLogger.Info($"Inventory export written: {filePath}");
        }

        public async Task ExportToCsvAsync(string filePath, List<InventoryItem> items, bool includeBom = true)
        {
            var content = BuildCsv(items);
            var encoding = new UTF8Encoding(includeBom);
            await File.WriteAllTextAsync(filePath, content, encoding);
            AppLogger.Info($"Inventory export written (async): {filePath}");
        }

        private static string BuildCsv(IEnumerable<InventoryItem> items)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Id;ItemName;CategoryName;BasePrice;FinalPrice");
            foreach (var item in items)
            {
                sb.AppendLine(
                    $"{item.Id};{EscapeCsv(item.ItemName)};{EscapeCsv(item.CategoryName)};" +
                    $"{item.BasePrice.ToString(CultureInfo.InvariantCulture)};" +
                    $"{item.FinalPrice.ToString(CultureInfo.InvariantCulture)}");
            }

            return sb.ToString();
        }

        private static string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            var mustQuote = value.Contains(';') || value.Contains('"') || value.Contains('\n') || value.Contains('\r');
            if (!mustQuote)
            {
                return value;
            }

            return $"\"{value.Replace("\"", "\"\"")}\"";
        }

        private static void ValidateMultiplierRange(decimal value, string paramName)
        {
            if (value < MinMultiplier || value > MaxMultiplier)
            {
                throw new ArgumentOutOfRangeException(paramName, $"Multiplier must be between {MinMultiplier} and {MaxMultiplier}.");
            }
        }
    }
}