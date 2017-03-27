using System;
using System.Collections.Generic;
using System.Linq;
using CashMushroom.Domain;

namespace CashMushroom.Queries
{
    public class BillBuilder :
        ISubscribeTo<CostsRecorded>
    {
        public Bill Bill { get; } = new Bill();

        public void Handle(CostsRecorded e)
        {
            foreach (var name in e.Payers)
            {
                var payer = Bill.Parties.SingleOrDefault(x => x.Name == name);
                if (payer == null) Bill.Parties.Add(payer = new Party {Name = name});
                payer.Total += e.Cost / e.Payers.Length;
            }
        }
    }
    
    public class Bill : IProjection
    {
        public List<Party> Parties { get; set; } = new List<Party>();
    }

    public class Party
    {
        public String Name { get; set; }
        public Decimal Total { get; set; }
    }
}