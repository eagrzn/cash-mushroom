using FrogsTalks.Domain;
using FrogsTalks.UseCases;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CashMushroom
{
    public class Product : Aggregate
    {
        public String Name { get; private set; }
        public Decimal Cost { get; private set; }
        public Friend Buyer { get; private set; }
        public IReadOnlyCollection<Friend> Payers { get; } = new List<Friend>();

        public IEnumerable Handle(RecordCosts c)
        {
            if (Version > 0) throw new ProductAlreadyPurchased();

            yield return new CostsRecorded
            {
                Id = c.Id,
                Name = c.Name,
                Cost = c.Cost,
                Buyer = c.Buyer,
                Payers = c.Payers
            };
        }

        public void Apply(CostsRecorded e)
        {
            Id = e.Id;
            Name = e.Name;
            Cost = e.Cost;
            Buyer = new Friend { Name = e.Buyer };
            ((List<Friend>)Payers).AddRange(e.Payers.Select(x => new Friend { Name = x }));
        }
    }

    public class RecordCosts : ICommand
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public Decimal Cost { get; set; }
        public String Buyer { get; set; }
        public String[] Payers { get; set; }
    }

    public class CostsRecorded : IEvent
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public Decimal Cost { get; set; }
        public String Buyer { get; set; }
        public String[] Payers { get; set; }
    }

    public class ProductAlreadyPurchased : Exception { }
}