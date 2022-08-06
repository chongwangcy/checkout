namespace Checkout.Domain
{
    public class MultipleItemsSpecialRule : SpecialRule
    {
        private readonly int _count;
        private readonly int _priceInCents;

        public MultipleItemsSpecialRule(Item item, int count, int priceInCents) : base(item)
        {
            _count = count;
            _priceInCents = priceInCents;
        }

        public override float GetSpecialPrice(int quantity) =>
            (quantity / _count) * _priceInCents + (quantity % _count) * _item.PriceInCents;

        public override bool IsSpecial(int quantity) => quantity >= _count;
    }
}
