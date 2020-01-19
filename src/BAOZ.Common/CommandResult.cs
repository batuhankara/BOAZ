using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;

namespace BAOZ.Common
{
    public class CommandResult : ExecutionResult
    {
        public CommandResult(bool isSuccess, IAggregateRoot aggregateRoot)
        {
            IsSuccess = isSuccess;
            AggregateRoot = aggregateRoot;
        }

        public override bool IsSuccess { get; }
        public IAggregateRoot AggregateRoot { get; }
    }

}
