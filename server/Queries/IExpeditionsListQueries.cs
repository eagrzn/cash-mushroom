using System;

namespace CashMushroom.Queries
{
    public interface IExpeditionsListQueries
    {
        ExpeditionsList.Item[] GetExpeditions(String memberName);
    }
}