using System;
using System.Collections;

namespace CashMushroom.Domain
{
    public class Product : Aggregate, IHandleCommand<Purchase>, IHandleCommand<Want>, IApplyEvent<ProductPurchased>, IApplyEvent<CostsTaken>
    {
        public String Name { get; private set; }
        //public Friend Buyer { get; private set; }
        //public IReadOnlyCollection<Friend> Owners { get; } = new List<Friend>();

        public void Apply(ProductPurchased e)
        {
            Id = e.Id;
            Name = e.Name;
        }

        public void Apply(CostsTaken e) { }

        public IEnumerable Handle(Purchase c)
        {
            if (EventsLoaded > 0)
                throw new ProductAlreadyPurchased();

            yield return new ProductPurchased
            {
                Id = c.Id,
                Name = c.Name,
                Cost = c.Cost
            };
        }

        public IEnumerable Handle(Want c)
        {
            yield return new CostsTaken
            {
                Id = c.Id,
                FriendPhone = c.FriendPhone
            };
        }
    }

    public class Purchase : ICommand
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public Decimal Cost { get; set; }
    }
    public class Want : ICommand
    {
        public Guid Id { get; set; }
        public String FriendPhone { get; set; }
    }

    public class ProductPurchased : IDomainEvent
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public Decimal Cost { get; set; }
    }
    public class CostsTaken : IDomainEvent
    {
        public Guid Id { get; set; }
        public String FriendPhone { get; set; }
    }

    public class ProductAlreadyPurchased : Exception { }
}