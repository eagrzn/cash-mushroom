using System;
using NUnit.Framework;

namespace CashMushroom.Domain
{
    [TestFixture]
    public class ProductTests : AggregateTests<Product>
    {
        private readonly Guid _1 = Guid.NewGuid();
        private readonly Guid _2 = Guid.NewGuid();
        private const String _whiskey = "Jack Daniel's";
        private const Decimal _2k = 2000;
        private const String _bob = "Bob";
        private const String _sam = "Sam";

        [Test]
        public void BobCanPurchaseWhiskey()
        {
            var purchase = new Purchase
            {
                Id = _1,
                ExpeditionId = _2,
                Name = _whiskey,
                Cost = _2k,
                BuyerName = _bob,
                BuyerTookCosts = true
            };
            var purchased = new ProductPurchased
            {
                Id = _1,
                ExpeditionId = _2,
                Name = _whiskey,
                Cost = _2k,
                BuyerName = _bob,
                BuyerTookCosts = true
            };

            Test(Given(), When(purchase), Then(purchased));
        }

        [Test]
        public void BobCantPurchaseOneWhiskeyTwice()
        {
            var purchased = new ProductPurchased
            {
                Id = _1,
                ExpeditionId = _2,
                Name = _whiskey,
                Cost = _2k,
                BuyerName = _bob,
                BuyerTookCosts = true
            };
            var purchase = new Purchase
            {
                Id = _1,
                ExpeditionId = _2,
                Name = _whiskey,
                Cost = _2k,
                BuyerName = _bob,
                BuyerTookCosts = true
            };

            Test(Given(purchased), When(purchase), ThenFailWith<ProductAlreadyPurchased>());
        }

        [Test]
        public void SamCanShareCostsWithBob()
        {
            var purchased = new ProductPurchased
            {
                Id = _1,
                ExpeditionId = _2,
                Name = _whiskey,
                Cost = _2k,
                BuyerName = _bob,
                BuyerTookCosts = true
            };
            var want = new Want
            {
                Id = _1,
                PayerName = _sam
            };
            var costsTaken = new CostsTaken
            {
                Id = _1,
                PayerName = _sam
            };

            Test(Given(purchased), When(want), Then(costsTaken));
        }

        [Test]
        public void SamCanTakeAllCosts()
        {
            var purchased = new ProductPurchased
            {
                Id = _1,
                ExpeditionId = _2,
                Name = _whiskey,
                Cost = _2k,
                BuyerName = _bob,
                BuyerTookCosts = false
            };
            var want = new Want
            {
                Id = _1,
                PayerName = _sam
            };
            var costsTaken = new CostsTaken
            {
                Id = _1,
                PayerName = _sam
            };

            Test(Given(purchased), When(want), Then(costsTaken));
        }

        [Test]
        public void SamCantTakeCostsTwice()
        {
            var purchased = new ProductPurchased
            {
                Id = _1,
                ExpeditionId = _2,
                Name = _whiskey,
                Cost = _2k,
                BuyerName = _bob,
                BuyerTookCosts = false
            };
            var costsTaken = new CostsTaken
            {
                Id = _1,
                PayerName = _sam
            };
            var want = new Want
            {
                Id = _1,
                PayerName = _sam
            };

            Test(Given(purchased, costsTaken), When(want), ThenFailWith<FriendAlreadyTookCosts>());
        }
    }
}