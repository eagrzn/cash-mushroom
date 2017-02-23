using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CashMushroom.Domain
{
    public class Product : Aggregate,
        IHandleCommand<Purchase>,
        IHandleCommand<Want>,
        IApplyEvent<ProductPurchased>,
        IApplyEvent<CostsTaken>
    {
        public String Name { get; private set; }
        public Decimal Cost { get; private set; }
        public Friend Buyer { get; private set; }
        public IReadOnlyCollection<Friend> Payers { get; } = new List<Friend>();

        public IEnumerable Handle(Purchase c)
        {
            if (EventsLoaded > 0) throw new ProductAlreadyPurchased();

            yield return new ProductPurchased
            {
                Id = c.Id,
                ExpeditionId = c.ExpeditionId,
                Name = c.Name,
                Cost = c.Cost,
                BuyerName = c.BuyerName,
                BuyerTookCosts = c.BuyerTookCosts
            };
        }

        public IEnumerable Handle(Want c)
        {
            if (Payers.Any(f => f.Name == c.PayerName)) throw new FriendAlreadyTookCosts();

            yield return new CostsTaken
            {
                Id = c.Id,
                PayerName = c.PayerName
            };
        }

        public void Apply(ProductPurchased e)
        {
            Id = e.Id;
            Name = e.Name;
            Cost = e.Cost;
            Buyer = new Friend { Name = e.BuyerName };
            if (e.BuyerTookCosts) (Payers as List<Friend>).Add(Buyer);
        }

        public void Apply(CostsTaken e)
        {
            var payer = new Friend { Name = e.PayerName };
            (Payers as List<Friend>).Add(payer);
        }
    }

    public class Purchase : ICommand
    {
        public Guid Id { get; set; }
        public Guid ExpeditionId { get; set; }
        public String Name { get; set; }
        public Decimal Cost { get; set; }
        public String BuyerName { get; set; }
        public bool BuyerTookCosts { get; set; }
    }
    public class Want : ICommand
    {
        public Guid Id { get; set; }
        public String PayerName { get; set; }
    }

    public class ProductPurchased : IDomainEvent
    {
        public Guid Id { get; set; }
        public Guid ExpeditionId { get; set; }
        public String Name { get; set; }
        public Decimal Cost { get; set; }
        public String BuyerName { get; set; }
        public bool BuyerTookCosts { get; set; }
    }
    public class CostsTaken : IDomainEvent
    {
        public Guid Id { get; set; }
        public String PayerName { get; set; }
    }

    public class ProductAlreadyPurchased : Exception { }
    public class FriendAlreadyTookCosts : Exception { }
}