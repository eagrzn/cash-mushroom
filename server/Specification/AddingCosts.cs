using FluentAssertions;
using FrogsTalks.Application;
using FrogsTalks.Application.Ports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using TestStack.BDDfy;

namespace CashMushroom.Specification
{
    [Story(AsA = "As a user",
        IWant = "I want to record the costs",
        SoThat = "So that I can share them with others")]
    [TestClass]
    public class AddingCosts
    {
        [TestMethod]
        public void BobBuysSomethingForHimself()
        {
            this.When(_ => _.BobRecordsCostsForWhiskeyForHimself())
                .And(_ => _.SamRecordsNothing())
                .Then(_ => _.AllCostsAreOnBob())
                .BDDfy();
        }

        [TestMethod]
        public void BobAndSamBuySomethingForThemselvesSolely()
        {
            this.When(_ => _.BobRecordsCostsForWhiskeyForHimself())
                .And(_ => _.SamRecordsCostsForCandiesForHimself())
                .Then(_ => _.WhiskeyCostsAreOnBob())
                .And(_ => _.CandiesCostsAreOnSam())
                .BDDfy();
        }

        [TestMethod]
        public void BobBuysSomethingAndShareItWithSam()
        {
            this.When(_ => _.BobRecordsCostsForWhiskeyForEverybody())
                .And(_ => _.SamRecordsNothing())
                .Then(_ => _.HalfOfWhiskeyCostsAreOnBob())
                .And(_ => _.HalfOfWhiskeyCostsAreOnSam())
                .BDDfy();
        }

        [TestMethod]
        public void BobBuysSomethingForSam()
        {
            this.When(_ => _.BobRecordsCostsForWhiskeyForSam())
                .And(_ => _.SamRecordsNothing())
                .Then(_ => _.AllCostsAreOnSam())
                .BDDfy();
        }

        #region When steps

        void BobRecordsCostsForWhiskeyForHimself()
        {
            var cmd = new RecordCosts
            {
                Id = _1,
                Buyer = _bob,
                Payers = new[] { _bob },
                Cost = _2k,
                Name = _whiskey
            };
            App.Do(cmd);
        }

        void SamRecordsNothing() { }

        void SamRecordsCostsForCandiesForHimself()
        {
            var cmd = new RecordCosts
            {
                Id = _2,
                Buyer = _sam,
                Payers = new[] { _sam },
                Cost = _500,
                Name = _candies
            };
            App.Do(cmd);
        }

        void BobRecordsCostsForWhiskeyForEverybody()
        {
            var cmd = new RecordCosts
            {
                Id = _1,
                Buyer = _bob,
                Payers = new[] { _bob, _sam },
                Cost = _2k,
                Name = _whiskey
            };
            App.Do(cmd);
        }

        void BobRecordsCostsForWhiskeyForSam()
        {
            var cmd = new RecordCosts
            {
                Id = _1,
                Buyer = _bob,
                Payers = new[] { _sam },
                Cost = _2k,
                Name = _whiskey
            };
            App.Do(cmd);
        }

        #endregion

        #region Then steps

        void AllCostsAreOnBob()
        {
            var bill = App.Get<Bill>(Tenant.Id);
            var bobPart = bill.Parties.Single(x => x.Name == _bob);
            bobPart.Total.Should().Be(_2k);
            var otherParts = bill.Parties.Where(x => x.Name != _bob);
            otherParts.Sum(x => x.Total).Should().Be(0);
        }

        void WhiskeyCostsAreOnBob()
        {
            var bill = App.Get<Bill>(Tenant.Id);
            var bobPart = bill.Parties.Single(x => x.Name == _bob);
            bobPart.Total.Should().Be(_2k);
        }

        void HalfOfWhiskeyCostsAreOnBob()
        {
            var bill = App.Get<Bill>(Tenant.Id);
            var bobPart = bill.Parties.Single(x => x.Name == _bob);
            bobPart.Total.Should().Be(_2k / 2);
        }

        void CandiesCostsAreOnSam()
        {
            var bill = App.Get<Bill>(Tenant.Id);
            var samPart = bill.Parties.Single(x => x.Name == _sam);
            samPart.Total.Should().Be(_500);
        }

        void HalfOfWhiskeyCostsAreOnSam()
        {
            var bill = App.Get<Bill>(Tenant.Id);
            var samPart = bill.Parties.Single(x => x.Name == _sam);
            samPart.Total.Should().Be(_2k / 2);
        }

        void AllCostsAreOnSam()
        {
            var bill = App.Get<Bill>(Tenant.Id);
            var samPart = bill.Parties.Single(x => x.Name == _sam);
            samPart.Total.Should().Be(_2k);
            var otherParts = bill.Parties.Where(x => x.Name != _sam);
            otherParts.Sum(x => x.Total).Should().Be(0);
        }

        #endregion

        #region Stuff

        [TestInitialize]
        public void SetUp()
        {
            var writeDb = new InMemoryEventStore();
            var readDb = new InMemoryStateStore();
            var bus = new InMemoryBus();

            App = new CashMushroom(bus, readDb);
            new CashMushroomLogic(bus, writeDb);
            new CashMushroomProjections(bus, readDb);
        }

        protected ApplicationFacade App;

        protected readonly Guid _1 = Guid.NewGuid();
        protected readonly Guid _2 = Guid.NewGuid();
        protected const String _whiskey = "Jack Daniel's";
        protected const String _candies = "Skittles";
        protected const Decimal _2k = 2000;
        protected const Decimal _500 = 500;
        protected const String _bob = "Bob";
        protected const String _sam = "Sam";

        #endregion
    }
}