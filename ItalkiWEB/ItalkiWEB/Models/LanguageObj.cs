using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ItalkiWEB.Models
{
    public class LanguageObj
    {
        public string language { get; set; }
        public int level { get; set; }
        public int priority { get; set; }
        public int is_teaching { get; set; }
        public int is_learning { get; set; }
        public int id { get; set; }
    }
}