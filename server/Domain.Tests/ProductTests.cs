using System;
using NUnit.Framework;

namespace CashMushroom.Domain
{
    [TestFixture]
    public class ProductTests : AggregateTests<Product>
    {
        private Guid _id;
        private String _name;
        private Decimal _cost;
        private String _friendPhone;

        [SetUp]
        public void Setup()
        {
            _id = Guid.NewGuid();
            _name = "Whiskey";
            _cost = 2000;
            _friendPhone = "123456789";
        }

        [Test]
        public void CanPurchase()
        {
            var purchase = new Purchase
            {
                Id = _id,
                Name = _name,
                Cost = _cost
            };
            var purchased = new ProductPurchased
            {
                Id = _id,
                Name = _name,
                Cost = _cost
            };

            Test(Given(), When(purchase), Then(purchased));
        }

        [Test]
        public void CantPurchaseTwice()
        {
            var purchased = new ProductPurchased
            {
                Id = _id,
                Name = _name,
                Cost = _cost
            };
            var purchase = new Purchase
            {
                Id = _id,
                Name = _name,
                Cost = _cost
            };

            Test(Given(purchased), When(purchase), ThenFailWith<ProductAlreadyPurchased>());
        }

        [Test]
        public void CostsCanBeShared()
        {
            var purchased = new ProductPurchased
            {
                Id = _id,
                Name = _name,
                Cost = _cost
            };
            var want = new Want
            {
                Id = _id,
                FriendPhone = _friendPhone
            };
            var costsTaken = new CostsTaken
            {
                Id = _id,
                FriendPhone = _friendPhone
            };

            Test(Given(purchased), When(want), Then(costsTaken));
        }
    }
}