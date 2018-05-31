using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ItalkiWEB.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ranking { get; set; }
        public int LessonsNumber { get; set; }
        public int StudentsNumber { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public List<LanguageObj> Languages { get; set; } = new List<LanguageObj>();
        public List<Object> Tags { get; set; }
        public List<String> Tags_string { get; set; } = new List<string>();
        public List<PersonalTag> TagsList { get; set; } = new List<PersonalTag>();
    }
}