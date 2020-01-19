using EventFlow.Core;
using System;
using System.Text.RegularExpressions;

namespace Baoz.Infrastructure.EventStore
{
    public class EventStoreStreamNameFactory : IEventStoreStreamNameFactory
    {

        public EventStoreStreamNameFactory()
        {
        }

        public string GetStreamName(Type aggregate, IIdentity identity)
        {
            string aggregateRootTypeName = GetFirstNonAbstractTypeInInheritenceHierarchy(aggregate).Name;

            string aggregateNameWithoutPostfix = new Regex("Aggregate$", RegexOptions.IgnoreCase).Replace(aggregateRootTypeName, string.Empty);

            //TODO:
            return $"{aggregateNameWithoutPostfix}-{identity.Value}";
        }

        public string GetAggregateName(string aggregateName)
        {
            string aggregateNameWithoutPostfix = new Regex("Aggregate$", RegexOptions.IgnoreCase).Replace(aggregateName, string.Empty);

            return aggregateNameWithoutPostfix;
        }

        private static Type GetFirstNonAbstractTypeInInheritenceHierarchy(Type aDerivedType)
        {
            Type baseType = aDerivedType.BaseType;

            if (baseType != null && !baseType.IsAbstract)
            {
                return GetFirstNonAbstractTypeInInheritenceHierarchy(baseType);
            }
            else
            {
                return aDerivedType;
            }
        }
    }

}
