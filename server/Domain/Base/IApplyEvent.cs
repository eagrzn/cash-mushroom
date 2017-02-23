namespace CashMushroom.Domain
{
    /// <summary>
    /// Implemented by an aggregate once for each event type it can apply.
    /// </summary>
    public interface IApplyEvent<E> where E : IDomainEvent
    {
        void Apply(E e);
    }
}