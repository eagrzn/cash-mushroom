using System;
using System.Collections.Generic;

namespace CashMushroom.Domain
{
    public class CashMushroom : IAggregateRoot
    {
        public IReadOnlyCollection<Friend> Friends { get; } = new List<Friend>();
    }
    public class Friend : IValueObject
    {
        public Guid Id { get; set; }
        public string Phone { get; set; }
    }

    public class Join : ICommand
    {
        public Guid FriendId { get; set; }
        public string Phone { get; set; }
    }

    public class FriendJoined : IDomainEvent
    {
        public Guid FriendId { get; set; }
        public string Phone { get; set; }
    }

    public class PhoneAlreadyExists : IException { }
}