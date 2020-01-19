using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using EventFlow.Core;
using Newtonsoft.Json;
using System;

namespace BAOZ.Common
{
    public abstract class BaseCommand<TAggregate, TIdentity, TExecutionResult> : Command<TAggregate, TIdentity, TExecutionResult>
    where TAggregate : IAggregateRoot<TIdentity>
    where TIdentity : IIdentity
    where TExecutionResult : IExecutionResult
    {
        protected BaseCommand(TIdentity aggregateId) : base(aggregateId)
        {

        }



        [JsonIgnore]
        public new TIdentity AggregateId { get => base.AggregateId; }

        private static Type GetFirstNonAbstractTypeInInheritenceHierarchy(Type aDerivedType)
        {
            var baseType = aDerivedType.BaseType;
            if (baseType != null && !baseType.IsAbstract)
            {
                return GetFirstNonAbstractTypeInInheritenceHierarchy(baseType);
            }
            else
            {
                return aDerivedType;
            }
        }

        public virtual ValidationResult Validate()
        {
            return new ValidationResult();
        }

        //public virtual Guid CommandIdentityToken
        //{
        //    get => Guid.Parse(base.GetSourceId().Value); 
        //}
    }


}
