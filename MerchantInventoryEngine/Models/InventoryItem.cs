namespace MerchantInventoryEngine.Models
{
    public class InventoryItem
    {
        public int Id { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public decimal FinalPrice { get; set; }
        public string PriceExplanation { get; set; } = string.Empty;
    }
}