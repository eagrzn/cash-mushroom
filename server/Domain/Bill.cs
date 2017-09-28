using FrogsTalks.Domain;
using System;
using System.Collections.Generic;

namespace CashMushroom.Queries
{
    public class Bill : IProjection
    {
        public Guid Id { get; set; }
        public List<Party> Parties { get; set; } = new List<Party>();
    }

    public class Party
    {
        public String Name { get; set; }
        public Decimal Total { get; set; }
    }
}