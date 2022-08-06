using Checkout.Domain;
using Checkout.Repositories;

namespace Checkout.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IDictionary<string, SpecialRule> _specialRules;
        private readonly List<Item> _items = new List<Item>();

        public CheckoutService(IItemRepository itemRepository, IEnumerable<SpecialRule> specialRules)
        {
            _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));

            if (specialRules == null)
                throw new ArgumentNullException(nameof(specialRules));
            _specialRules = specialRules.ToDictionary(rule => rule.Sku, rule => rule);
        }

        public float GetTotalPrice()
        {
            var totalPrice = 0.0f;
            var groupedItems = _items.GroupBy(i => i.Sku);

            foreach (var groupedItem in groupedItems)
            {
                var quantity = groupedItem.Count();
                if (_specialRules.TryGetValue(groupedItem.Key, out var specialRule) &&
                    specialRule.IsSpecial(quantity))
                {
                    totalPrice += specialRule.GetSpecialPrice(quantity);
                }
                else
                {
                    totalPrice += groupedItem.First().PriceInCents * quantity;
                }
            }

            return totalPrice;
        }

        public void Scan(string sku)
        {
            if (!string.IsNullOrEmpty(sku))
            {
                var item = _itemRepository.GetItem(sku);
                _items.Add(item);
            }
        }
    }
}
