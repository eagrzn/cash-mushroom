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

        public T Get<T>() where T : class, IProjection
        {
            if (typeof(T) == typeof(Bill)) return _billBuilder.Bill as T;

            throw new NotImplementedException();
        }

        public CashMushroom()
        {
            _dispatcher = new MessageDispatcher(new InMemoryEventStore());

            _dispatcher.ScanInstance(new Product());

            _billBuilder = new BillBuilder();
            _dispatcher.ScanInstance(_billBuilder);
        }

        private readonly MessageDispatcher _dispatcher;
        private readonly BillBuilder _billBuilder;
    }
}