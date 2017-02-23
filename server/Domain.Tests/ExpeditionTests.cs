using System;
using NUnit.Framework;

namespace CashMushroom.Domain
{
    [TestFixture]
    public class ExpeditionTests : AggregateTests<Expedition>
    {
        private Guid _1;
        private String _bob;
        private String _sam;

        [SetUp]
        public void Setup()
        {
            _1 = Guid.NewGuid();
            _bob = "Bob";
            _sam = "Sam";
        }

        [Test]
        public void BobCanJoin()
        {
            var join = new Join
            {
                Id = _1,
                Name = _bob
            };
            var joined = new FriendJoined
            {
                Id = _1,
                Name = _bob
            };

            Test(Given(), When(join), Then(joined));
        }

        [Test]
        public void BobAndSamCanJoin()
        {
            var bobJoined = new FriendJoined
            {
                Id = _1,
                Name = _bob
            };
            var joinSam = new Join
            {
                Id = _1,
                Name = _sam
            };
            var samJoined = new FriendJoined
            {
                Id = _1,
                Name = _sam
            };

            Test(Given(bobJoined), When(joinSam), Then(samJoined));
        }

        [Test]
        public void TwoBobsCantJoin()
        {
            var joined = new FriendJoined
            {
                Id = _1,
                Name = _bob
            };
            var join = new Join
            {
                Id = _1,
                Name = _bob
            };

            Test(Given(joined), When(join), ThenFailWith<NameAlreadyInvolved>());
        }
    }
}