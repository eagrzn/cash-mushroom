using System;
using CashMushroom.Domain;
using CashMushroom.Queries;

namespace CashMushroom.Application
{
    public class CashMushroom
    {
        public void Do(ICommand command)
        {
            _dispatcher.SendCommand(command);
        }

        public Expeditions.Expedition[] GetFriendExpeditions(String friendName)
        {
            return _expeditions.GetByFriend(friendName);
        }

        public Expeditions.Product[] GetExpeditionProducts(Guid expeditionId)
        {
            return _expeditions.GetExpeditionProducts(expeditionId);
        }

        public CashMushroom()
        {
            _dispatcher = new MessageDispatcher(new InMemoryEventStore());

            _dispatcher.ScanInstance(new Expedition());
            _dispatcher.ScanInstance(new Product());

            _expeditions = new Expeditions();
            _dispatcher.ScanInstance(_expeditions);
        }

        private readonly MessageDispatcher _dispatcher;
        private readonly Expeditions _expeditions;
    }
}