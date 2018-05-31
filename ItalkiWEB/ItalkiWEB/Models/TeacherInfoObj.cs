using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ItalkiWEB.Models
{
    public class TeacherInfoObj
    {
        public string video_url { get; set; }
        public List<List<int>> available_schedule { get; set; }
        public string teacher_tags { get; set; }
        public string intro { get; set; }
        public string about_me { get; set; }
        public string teacher_child_tags { get; set; }
        public string pro_rating { get; set; }
        public int student_count { get; set; }
        public string tutor_rating { get; set; }
        public int comment_count { get; set; }
        public int instant_tutoring_price { get; set; }
        public int min_price { get; set; }
        public int has_trial { get; set; }
        public int is_available_instant_tutoring { get; set; }
        public DateTime first_valid_time { get; set; }
        public int instant_tutoring_status { get; set; }
        public int trial_price_usd { get; set; }
        public int user_id { get; set; }
        public int instant_tutoring_price_usd { get; set; }
        public int trial_price { get; set; }
        public int min_price_usd { get; set; }
        public int session_count { get; set; }
        public int max_price_usd { get; set; }
        public int max_price { get; set; }
    }
}