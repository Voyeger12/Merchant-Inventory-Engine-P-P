namespace MerchantInventoryEngine.Models
{
    public class Modifier
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Multiplier { get; set; }
        public string Type { get; set; } = string.Empty; // e.g., "Personality", "Location", "Political"
    }
}