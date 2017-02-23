using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CashMushroom.Domain
{
    public class CashMushroom : Aggregate, IHandleCommand<Join>, IApplyEvent<FriendJoined>
    {
        public IReadOnlyCollection<Friend> Friends { get; } = new List<Friend>();

        public void Apply(FriendJoined e)
        {
            var joined = new Friend { Phone = e.Phone };
            (Friends as List<Friend>).Add(joined);
        }

        public IEnumerable Handle(Join c)
        {
            if (Friends.Any(f => f.Phone == c.Phone))
                throw new PhoneAlreadyInvolved();

            yield return new FriendJoined { Phone = c.Phone };
        }
    }
    public class Friend : IValueObject
    {
        public String Phone { get; set; }
    }

    public class Join : ICommand
    {
        public String Phone { get; set; }
    }

    public class FriendJoined : IDomainEvent
    {
        public String Phone { get; set; }
    }

    public class PhoneAlreadyInvolved : Exception { }
}