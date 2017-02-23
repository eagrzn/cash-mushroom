using System;
using NUnit.Framework;

namespace CashMushroom.Domain
{
    [TestFixture]
    public class CashMushroomTests : AggregateTests<CashMushroom>
    {
        private String _phone;

        [SetUp]
        public void Setup()
        {
            _phone = "123456789";
        }

        [Test]
        public void FriendCanJoin()
        {
            Test(
                Given(),
                When(new Join { Phone = _phone }),
                Then(new FriendJoined { Phone = _phone })
            );
        }

        [Test]
        public void TwoFriendsCantHaveSamePhone()
        {
            Test(
                Given(new FriendJoined { Phone = _phone }),
                When(new Join { Phone = _phone }),
                ThenFailWith<PhoneAlreadyInvolved>()
            );
        }
    }
}