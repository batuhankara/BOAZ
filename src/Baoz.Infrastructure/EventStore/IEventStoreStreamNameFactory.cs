using EventFlow.Core;
using System;

namespace Baoz.Infrastructure.EventStore
{
    public interface IEventStoreStreamNameFactory
    {
        string GetStreamName(Type aggregate, IIdentity identity);
        string GetAggregateName(string aggregateName);
    }

}
