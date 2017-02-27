using System;
using System.Collections.Generic;
using System.Linq;
using CashMushroom.Domain;

namespace CashMushroom.Queries
{
    public class Expeditions :
        ISubscribeTo<ExpeditionStarted>,
        ISubscribeTo<FriendJoined>,
        ISubscribeTo<ProductPurchased>,
        ISubscribeTo<CostsTaken>
    {
        public Expedition[] GetByFriend(String friendName)
        {
            if (!_friendExpeditions.ContainsKey(friendName)) return new Expedition[0];

            var result = from fe in _friendExpeditions[friendName]
                         join e in _expeditions on fe equals e.Key
                         select new Expedition { Id = fe, Name = e.Value };
            return result.ToArray();
        }

        public Product[] GetExpeditionProducts(Guid id)
        {
            var products = _expeditionProducts.ContainsKey(id) ? _expeditionProducts[id] : new List<Product>();
            return products.ToArray();
        }

        public void Handle(ExpeditionStarted e)
        {
            _expeditions.Add(e.Id, e.Name);
            _expeditionProducts.Add(e.Id, new List<Product>());
        }

        public void Handle(FriendJoined e)
        {
            if (!_friendExpeditions.ContainsKey(e.FriendName)) _friendExpeditions.Add(e.FriendName, new List<Guid>());
            _friendExpeditions[e.FriendName].Add(e.Id);
        }

        public void Handle(ProductPurchased e)
        {
            var product = new Product
            {
                Id = e.Id,
                Name = e.Name,
                Cost = e.Cost,
                PayersCount = (Byte)(e.BuyerTookCosts ? 1 : 0)
            };
            _expeditionProducts[e.ExpeditionId].Add(product);
        }

        public void Handle(CostsTaken e)
        {
            var expedition = _expeditionProducts.Single(x => x.Value.Any(y => y.Id == e.Id));
            _expeditionProducts[expedition.Key].Single(p => p.Id == e.Id).PayersCount++;
        }

        public class Expedition
        {
            public Guid Id { get; set; }
            public String Name { get; set; }
        }

        public class Product
        {
            public Guid Id { get; set; }
            public String Name { get; set; }
            public Decimal Cost { get; set; }
            public Byte PayersCount { get; set; }
        }

        private readonly Dictionary<Guid, String> _expeditions = new Dictionary<Guid, String>();
        private readonly Dictionary<String, List<Guid>> _friendExpeditions = new Dictionary<String, List<Guid>>();
        private readonly Dictionary<Guid, List<Product>> _expeditionProducts = new Dictionary<Guid, List<Product>>();
    }
}