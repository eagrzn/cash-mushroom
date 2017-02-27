using System;
using NUnit.Framework;

namespace CashMushroom.Domain
{
    [TestFixture]
    public class ExpeditionTests : AggregateTests<Expedition>
    {
        private readonly Guid _1 = Guid.NewGuid();
        private const String _bob = "Bob";
        private const String _sam = "Sam";

        [Test]
        public void BobCanJoin()
        {
            var join = new Join
            {
                Id = _1,
                FriendName = _bob
            };
            var started = new ExpeditionStarted
            {
                Id = _1,
                Name = Expedition.GetName(_1, _bob)
            };
            var joined = new FriendJoined
            {
                Id = _1,
                FriendName = _bob
            };

            Test(Given(), When(join), Then(started, joined));
        }

        [Test]
        public void BobAndSamCanJoin()
        {
            var bobJoined = new FriendJoined
            {
                Id = _1,
                FriendName = _bob
            };
            var joinSam = new Join
            {
                Id = _1,
                FriendName = _sam
            };
            var samJoined = new FriendJoined
            {
                Id = _1,
                FriendName = _sam
            };

            Test(Given(bobJoined), When(joinSam), Then(samJoined));
        }

        [Test]
        public void TwoBobsCantJoin()
        {
            var joined = new FriendJoined
            {
                Id = _1,
                FriendName = _bob
            };
            var join = new Join
            {
                Id = _1,
                FriendName = _bob
            };

            Test(Given(joined), When(join), ThenFailWith<FriendAlreadyJoined>());
        }
    }
}