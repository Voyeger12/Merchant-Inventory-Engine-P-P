namespace MerchantInventoryEngine.Services
{
    public class PriceCalculator
    {
        public decimal CalculateFinalPrice(decimal basePrice, decimal personalityMultiplier, decimal locationMultiplier, decimal politicalMultiplier)
        {
            return basePrice * personalityMultiplier * locationMultiplier * politicalMultiplier;
        }
    }
}