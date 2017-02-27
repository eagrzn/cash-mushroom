using System.Collections;

namespace CashMushroom.Domain
{
    public interface IHandleCommand<T> where T : ICommand
    {
        IEnumerable Handle(T c);
    }
}