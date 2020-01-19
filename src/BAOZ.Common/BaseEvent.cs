using EventFlow.Aggregates;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAOZ.Common
{
    public abstract class BaseEvent<TAggregate> : AggregateEvent<TAggregate, BaozId>
     where TAggregate : IAggregateRoot<BaozId>
    {
        [JsonConstructor]

        public BaseEvent()
        {
            OccuredOnUTC = DateTime.UtcNow;
        }
        public DateTime OccuredOnUTC
        {
            get; set;
        }
    }

}
