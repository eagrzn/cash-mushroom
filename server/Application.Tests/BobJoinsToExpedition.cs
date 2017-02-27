using System;
using CashMushroom.Domain;
using NUnit.Framework;
using TestStack.BDDfy;

namespace CashMushroom.Application
{
    public class BobJoinsToExpedition
    {
        private CashMushroom _app;

        private Guid _1 = Guid.NewGuid();
        private String _bob = "Bob";

        void WhenBobJoinsToNewExpedition()
        {
            _app.Do(new Join { Id = _1, FriendName = _bob });
        }

        void ThenHeSeesItInHisExpeditionsList()
        {
            var expeditions = _app.GetExpeditionsList(_bob);
            Assert.AreEqual(1, expeditions.Length);
            Assert.AreEqual(_1, expeditions[0].Id);
        }

        [Test]
        public void Execute()
        {
            _app = new CashMushroom();
            this.BDDfy();
        }
    }
}