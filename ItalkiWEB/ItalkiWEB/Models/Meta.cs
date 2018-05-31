using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ItalkiWEB.Models
{
    public class Meta
    {
        public bool has_next { get; set; }
        public int current_page { get; set; }
        public int page_size { get; set; }
        public StatisticsInfo statistics_info { get; set; }
    }
}