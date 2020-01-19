using BAOZ.Common.Converters;
using EventFlow.Core;
using Newtonsoft.Json;

namespace BAOZ.Common
{
    [JsonConverter(typeof(IdentityObjectConverter))]
    public class BaozId : IIdentity
    {
        public BaozId(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
    }
}
