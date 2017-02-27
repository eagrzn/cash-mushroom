using System;
using System.Linq;
using CashMushroom.Domain;
using NUnit.Framework;
using TestStack.BDDfy;

namespace CashMushroom.Application
{
    [Story(AsA = "As a user",
           IWant = "I want to record expeditions costs",
           SoThat = "So that I can share them with my friends")]
    public class CostsManagement : Feature
    {
        void BobJoinsToExpedition()
        {
            App.Do(new Join { Id = _1, FriendName = _bob });
        }

        void SamJoinsToExpedition()
        {
            App.Do(new Join { Id = _1, FriendName = _sam });
        }

        void BobBuysWhiskey(Boolean tookCosts)
        {
            var purchase = new Purchase
            {
                Id = _2,
                ExpeditionId = _1,
                BuyerName = _bob,
                BuyerTookCosts = tookCosts,
                Cost = _2k,
                Name = _whiskey
            };
            App.Do(purchase);
        }

        void SamBuysCandies(Boolean tookCosts)
        {
            var purchase = new Purchase
            {
                Id = _3,
                ExpeditionId = _1,
                BuyerName = _bob,
                BuyerTookCosts = tookCosts,
                Cost = _500,
                Name = _candies
            };
            App.Do(purchase);
        }

        void SamTakeCostsForWhiskey()
        {
            App.Do(new Want { Id = _2, PayerName = _sam });
        }

        void BobSeesExpeditionInList()
        {
            var expeditions = App.GetFriendExpeditions(_bob);
            Assert.AreEqual(1, expeditions.Length);
            Assert.AreEqual(_1, expeditions[0].Id);
        }

        void BobSeesWhiskeyInExpeditionProducts(Byte payers)
        {
            var whiskey = App.GetExpeditionProducts(_1).Single(x => x.Name == _whiskey);
            Assert.AreEqual(_2, whiskey.Id);
            Assert.AreEqual(_2k, whiskey.Cost);
            Assert.AreEqual(payers, whiskey.PayersCount);
        }

        void BobSeesCandiesInExpeditionProducts(Byte payers)
        {
            var candies = App.GetExpeditionProducts(_1).Single(x => x.Name == _candies);
            Assert.AreEqual(_3, candies.Id);
            Assert.AreEqual(_500, candies.Cost);
            Assert.AreEqual(payers, candies.PayersCount);
        }

        [Test]
        public void BobStartsExpedition()
        {
            this.When(x => x.BobJoinsToExpedition())
                .Then(x => x.BobSeesExpeditionInList())
                .BDDfy();
        }

        [Test]
        public void BobBuysProductForHimself()
        {
            this.Given(x => x.BobJoinsToExpedition())
                .When(x => x.BobBuysWhiskey(true))
                .Then(x => x.BobSeesWhiskeyInExpeditionProducts(1))
                .BDDfy();
        }

        [Test]
        public void BobAndSamBuyProductForThemselvesSolely()
        {
            this.Given(x => x.BobJoinsToExpedition())
                .Given(x => x.SamJoinsToExpedition())
                .When(x => x.BobBuysWhiskey(true))
                .When(x => x.SamBuysCandies(true))
                .Then(x => x.BobSeesWhiskeyInExpeditionProducts(1))
                .Then(x => x.BobSeesCandiesInExpeditionProducts(1))
                .BDDfy();
        }

        [Test]
        public void BobBuysProductAndShareItWithSam()
        {
            this.Given(x => x.BobJoinsToExpedition())
                .Given(x => x.SamJoinsToExpedition())
                .When(x => x.BobBuysWhiskey(true))
                .When(x => x.SamTakeCostsForWhiskey())
                .Then(x => x.BobSeesWhiskeyInExpeditionProducts(2))
                .BDDfy();
        }

        [Test]
        public void BobBuysProductForSam()
        {
            this.Given(x => x.BobJoinsToExpedition())
                .Given(x => x.SamJoinsToExpedition())
                .When(x => x.BobBuysWhiskey(false))
                .When(x => x.SamTakeCostsForWhiskey())
                .Then(x => x.BobSeesWhiskeyInExpeditionProducts(1))
                .BDDfy();
        }
    }
}