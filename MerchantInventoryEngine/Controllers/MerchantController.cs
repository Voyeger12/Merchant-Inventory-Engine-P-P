using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MerchantInventoryEngine.Data;
using MerchantInventoryEngine.Models;
using MerchantInventoryEngine.Services;

namespace MerchantInventoryEngine.Controllers
{
    public class MerchantController
    {
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

        public List<InventoryItem> CalculateInventory(decimal personalityMultiplier, decimal locationMultiplier, decimal politicalMultiplier)
        {
            var items = _db.GetItems();
            var categories = _db.GetCategories().ToDictionary(c => c.Id, c => c.Name);
            var inventory = new List<InventoryItem>();

            foreach(var item in items)
            {
                var final = _calculator.CalculateFinalPrice(item.BasePrice, personalityMultiplier, locationMultiplier, politicalMultiplier);
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

        public void ExportToCsv(string filePath, List<InventoryItem> items)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Id;ItemName;CategoryName;BasePrice;FinalPrice");
            foreach(var item in items)
            {
                sb.AppendLine($"{item.Id};{item.ItemName};{item.CategoryName};{item.BasePrice};{item.FinalPrice}");
            }
            File.WriteAllText(filePath, sb.ToString());
        }
    }
}