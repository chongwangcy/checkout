namespace Checkout.Domain
{
    public abstract class SpecialRule
    {
        protected readonly Item _item;
        public SpecialRule(Item item)
        {
            _item = item;
        }

        public string Sku
        {
            get => _item.Sku;
        }

        public abstract bool IsSpecial(int quantity);

        public abstract float GetSpecialPrice(int quantity);
    }
}
