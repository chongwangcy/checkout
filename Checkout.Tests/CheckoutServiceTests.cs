using Checkout.Domain;
using Checkout.Repositories;
using Checkout.Services;
using Moq;

namespace Checkout.Tests
{
    public class CheckoutServiceTests
    {
        private readonly Mock<IItemRepository> _itemRepository = new Mock<IItemRepository>();
        private readonly IEnumerable<SpecialRule> _rules;

        public CheckoutServiceTests()
        {
            var items = new Item[]
            {
                new Item("A", 50),
                new Item("B", 30),
                new Item("C", 20),
                new Item("D", 15)
            };

            _itemRepository.Setup(i => i.GetItem("A")).Returns(items[0]);
            _itemRepository.Setup(i => i.GetItem("B")).Returns(items[1]);
            _itemRepository.Setup(i => i.GetItem("C")).Returns(items[2]);
            _itemRepository.Setup(i => i.GetItem("D")).Returns(items[3]);

            _rules = new List<SpecialRule>
            { 
                new MultipleItemsSpecialRule(items[0], 3, 130),
                new MultipleItemsSpecialRule(items[1], 2, 45)
            };
        }


        [Fact]
        public void WhenNoOrEmptyItemScannedTotalPriceIsZero()
        {
            var checkoutService = GetTestSubject(_rules);

            Assert.Equal(0, checkoutService.GetTotalPrice());

            checkoutService.Scan("");

            Assert.Equal(0, checkoutService.GetTotalPrice());
        }

        [Fact]
        public void WhenMultipleItemsRuleAppliedPriceIncrementCorrect()
        {
            var checkoutService = GetTestSubject(_rules);

            checkoutService.Scan("A");
            Assert.Equal(50, checkoutService.GetTotalPrice());

            checkoutService.Scan("B");
            Assert.Equal(80, checkoutService.GetTotalPrice());

            checkoutService.Scan("A");
            Assert.Equal(130, checkoutService.GetTotalPrice());

            checkoutService.Scan("A");
            Assert.Equal(160, checkoutService.GetTotalPrice());

            checkoutService.Scan("B");
            Assert.Equal(175, checkoutService.GetTotalPrice());
        }

        [Theory]
        [InlineData("A", 50)]
        [InlineData("A,B", 80)]
        [InlineData("C,D,B,A", 115)]
        [InlineData("A,A", 100)]
        [InlineData("A,A,A", 130)]
        [InlineData("A,A,A,A", 180)]
        [InlineData("A,A,A,A,A", 230)]
        [InlineData("A,A,A,A,A,A", 260)]
        [InlineData("A,A,A,B", 160)]
        [InlineData("A,A,A,B,B", 175)]
        [InlineData("A,A,A,B,B,D", 190)]
        [InlineData("D,A,B,A,B,A", 190)]
        public void WhenMultipleItemsRuleAppliedTotalPriceRespectsRules(string skuString, int expectedTotalPrice)
        {
            var checkoutService = GetTestSubject(_rules);

            foreach (var sku in skuString.Split(','))
            {
                checkoutService.Scan(sku);
            }

            Assert.Equal(expectedTotalPrice, checkoutService.GetTotalPrice());
        }

        private ICheckoutService GetTestSubject(IEnumerable<SpecialRule> rules) =>
            new CheckoutService(_itemRepository.Object, rules); 
    }
}