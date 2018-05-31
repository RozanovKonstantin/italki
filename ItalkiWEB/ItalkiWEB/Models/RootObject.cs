using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ItalkiWEB.Models
{
    public class RootObject
    {
        public string performance { get; set; }
        public Meta meta { get; set; }
        public long server_time { get; set; }
        public List<Datum> data { get; set; }
    }
}