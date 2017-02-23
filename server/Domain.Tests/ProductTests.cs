using System;
using NUnit.Framework;

namespace CashMushroom.Domain
{
    [TestFixture]
    public class ProductTests : AggregateTests<Product>
    {
        private Guid _1;
        private Guid _2;
        private String _whiskey;
        private Decimal _2k;
        private String _bob;
        private String _sam;

        [SetUp]
        public void Setup()
        {
            _1 = Guid.NewGuid();
            _2 = Guid.NewGuid();
            _whiskey = "Whiskey";
            _2k = 2000;
            _bob = "Bob";
            _sam = "Sam";
        }

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