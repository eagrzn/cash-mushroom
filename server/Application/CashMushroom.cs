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

        public ExpeditionsList.Item[] GetExpeditionsList(String friendName)
        {
            return _expeditionsList.GetExpeditions(friendName);
        }

        public CashMushroom()
        {
            _dispatcher = new MessageDispatcher(new InMemoryEventStore());

            _dispatcher.ScanInstance(new Expedition());
            _dispatcher.ScanInstance(new Product());

            _expeditionsList = new ExpeditionsList();
            _dispatcher.ScanInstance(_expeditionsList);
        }

        private readonly MessageDispatcher _dispatcher;
        private readonly ExpeditionsList _expeditionsList;
    }
}