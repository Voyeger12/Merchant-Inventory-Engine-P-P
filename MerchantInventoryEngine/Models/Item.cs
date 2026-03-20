namespace MerchantInventoryEngine.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public int CategoryId { get; set; }
    }
}