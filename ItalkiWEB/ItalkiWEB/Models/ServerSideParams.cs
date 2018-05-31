using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ItalkiWEB.Models
{
    public class ServerSideParams
    {
        public int page { get; set; }
        public int rows { get; set; }
        public string sidx { get; set; }
        public string sord { get; set; }
        public bool _search { get; set; }
        public string searchString { get; set; }
    }
}