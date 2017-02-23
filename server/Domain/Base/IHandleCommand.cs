using System.Collections;

namespace CashMushroom.Domain
{
    public interface IHandleCommand<C> where C : ICommand
    {
        IEnumerable Handle(C c);
    }
}