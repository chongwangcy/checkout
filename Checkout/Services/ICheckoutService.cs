namespace Checkout.Services
{
    public interface ICheckoutService
    {
        void Scan(string sku);
        float GetTotalPrice();
    }
}
