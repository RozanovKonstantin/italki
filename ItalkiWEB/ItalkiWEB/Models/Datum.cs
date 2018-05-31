using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ItalkiWEB.Models
{
    public class Datum
    {
        public int is_premium { get; set; }
        public object exam_result_shown_obj { get; set; }
        public int course_count { get; set; }
        public int favourite_flag { get; set; }
        public List<object> personal_tag { get; set; }
        public string textid { get; set; }
        public string nickname { get; set; }
        public int has_contacted { get; set; }
        public string avatar_file_name { get; set; }
        public TeacherInfoObj teacher_info_obj { get; set; }
        public int is_favourite { get; set; }
        public object cn_video_view_url { get; set; }
        public string origin_country_id { get; set; }
        public int is_online { get; set; }
        public int id { get; set; }
        public List<LanguageObj> language_obj_s { get; set; }
        public int is_tutor { get; set; }
        public int is_pro { get; set; }
        public string oms_apply_video_url { get; set; }
    }
}