using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CashMushroom.Domain
{
    public class Expedition : Aggregate,
        IHandleCommand<Join>,
        IApplyEvent<FriendJoined>
    {
        public IReadOnlyCollection<Friend> Members { get; } = new List<Friend>();

        public IEnumerable Handle(Join c)
        {
            if (Members.Any(f => f.Name == c.Name))
                throw new NameAlreadyInvolved();

            yield return new FriendJoined { Id = c.Id, Name = c.Name };
        }

        public void Apply(FriendJoined e)
        {
            var joined = new Friend { Name = e.Name };
            (Members as List<Friend>).Add(joined);
        }
    }

    public class Join : ICommand
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
    }

    public class FriendJoined : IDomainEvent
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
    }

    public class NameAlreadyInvolved : Exception { }
}