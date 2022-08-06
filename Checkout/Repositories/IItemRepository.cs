using Checkout.Domain;

namespace Checkout.Repositories
{
    public interface IItemRepository
    {
        Item GetItem(string sku);
    }
}
