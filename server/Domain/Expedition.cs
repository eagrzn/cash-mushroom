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
        public String Name { get; private set; }
        public IReadOnlyCollection<Friend> Members { get; } = new List<Friend>();

        public IEnumerable Handle(Join c)
        {
            if (Members.Any(f => f.Name == c.FriendName)) throw new FriendAlreadyJoined();

            if (EventsLoaded == 0)
                yield return new ExpeditionStarted { Id = c.Id, Name = GetName(c.Id, c.FriendName) };
            yield return new FriendJoined { Id = c.Id, FriendName = c.FriendName };
        }

        public void Apply(ExpeditionStarted e)
        {
            Id = e.Id;
            Name = e.Name;
        }

        public void Apply(FriendJoined e)
        {
            var joined = new Friend { Name = e.FriendName };
            (Members as List<Friend>).Add(joined);
        }

        public static String GetName(Guid id, String starterName) => $"{id.ToString().Substring(0, 2)} ({starterName})";
    }

    public class Join : ICommand
    {
        public Guid Id { get; set; }
        public String FriendName { get; set; }
    }

    public class ExpeditionStarted : IDomainEvent
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
    }
    public class FriendJoined : IDomainEvent
    {
        public Guid Id { get; set; }
        public String FriendName { get; set; }
    }

    public class FriendAlreadyJoined : Exception { }
}