using System;

namespace CashMushroom.Domain
{
    public class Product : IAggregateRoot { }

    public class Purchase : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
    }
    public class Want : ICommand
    {
        public Guid Id { get; set; }
        public Guid FriendId { get; set; }
    }

    public class ProductPurchased : IDomainEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
    }
    public class CostsTaken : IDomainEvent
    {
        public Guid Id { get; set; }
        public Guid FriendId { get; set; }
    }
}