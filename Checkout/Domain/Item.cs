namespace Checkout.Domain
{
    public class Item
    {
        public Item(string sku, int priceInCents)
        {
            Sku = sku;
            PriceInCents = priceInCents;
        }
    
        public string Sku { get; }

        public int PriceInCents { get; }
    }
}
