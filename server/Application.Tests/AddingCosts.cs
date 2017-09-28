using CashMushroom.Domain;
using CashMushroom.Queries;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using TestStack.BDDfy;

namespace CashMushroom.Application
{
    [Story(AsA = "As a party",
        IWant = "I want to record the costs",
        SoThat = "So that I can share them with others")]
    public class AddingCosts : Feature
    {
        [Test]
        public void BobBuysSomethingForHimself()
        {
            this.When(_ => _.BobRecordsCostsForWhiskeyForHimself())
                .And(_ => _.SamRecordsNothing())
                .Then(_ => _.AllCostsAreOnBob())
                .BDDfy();
        }

        [Test]
        public void BobAndSamBuySomethingForThemselvesSolely()
        {
            this.When(_ => _.BobRecordsCostsForWhiskeyForHimself())
                .And(_ => _.SamRecordsCostsForCandiesForHimself())
                .Then(_ => _.WhiskeyCostsAreOnBob())
                .And(_ => _.CandiesCostsAreOnSam())
                .BDDfy();
        }

        [Test]
        public void BobBuysSomethingAndShareItWithSam()
        {
            this.When(_ => _.BobRecordsCostsForWhiskeyForEverybody())
                .And(_ => _.SamRecordsNothing())
                .Then(_ => _.HalfOfWhiskeyCostsAreOnBob())
                .And(_ => _.HalfOfWhiskeyCostsAreOnSam())
                .BDDfy();
        }

        [Test]
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
    }
}