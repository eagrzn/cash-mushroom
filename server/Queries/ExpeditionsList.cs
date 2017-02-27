using System;
using System.Collections.Generic;
using System.Linq;
using CashMushroom.Domain;

namespace CashMushroom.Queries
{
    public class ExpeditionsList :
        ISubscribeTo<ExpeditionStarted>,
        ISubscribeTo<FriendJoined>
    {
        public Item[] GetExpeditions(String friendName)
        {
            if (!_friendExpeditions.ContainsKey(friendName)) return new Item[0];

            var result = from fe in _friendExpeditions[friendName]
                         join e in _expeditions on fe equals e.Key
                         select new Item { Id = fe, Name = e.Value };
            return result.ToArray();
        }

        public void Handle(ExpeditionStarted e)
        {
            _expeditions.Add(e.Id, e.Name);
        }

        public void Handle(FriendJoined e)
        {
            if (!_friendExpeditions.ContainsKey(e.FriendName)) _friendExpeditions.Add(e.FriendName, new List<Guid>());
            _friendExpeditions[e.FriendName].Add(e.Id);
        }

        public class Item
        {
            public Guid Id { get; set; }
            public String Name { get; set; }
        }

        private readonly Dictionary<String, List<Guid>> _friendExpeditions = new Dictionary<String, List<Guid>>();
        private readonly Dictionary<Guid, String> _expeditions = new Dictionary<Guid, String>();
    }
}