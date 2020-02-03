using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAOZ.Api.Models
{
    public class RequestLogModel
    {
        public string Url { get; set; }
        public List<KeyValuePair<string, string>> Headers { get; set; }
        public string Body { get; set; }

    }
}
