using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Configuration;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;

namespace ItalkiWEB.Models
{
    public class InfoManipulation
    {
        public static List<RootObject> GetRootList()
        {
            List<RootObject> list = new List<RootObject>();
            string adress = "https://www.italki.com/api/teachersv2?_r=1526449935451&country=&hl=ru-ru&i_token=TlRFNU9UTTNPUT09fDE1MjYzODA3MjV8Njk1ODljMTUwOWM5MzA5YTgwNzMzOTUwNTlhY2JhMGI2N2ViNjZhYw%3D%3D&is_advanced=&is_instant=&is_native=&is_trial=&is_video=&minute_between=&nick_keyword=&page=1&price_usd=&q=&speak=&teach=english&teacher_type=0";
            WebRequest request;
            WebResponse response;
            RootObject rootObj = new RootObject();
            string res;
            int pageNum = 1;
            while (true)
            {
                request = (HttpWebRequest)WebRequest.Create(adress);
                request.ContentType = "text/json";
                request.Method = "GET";
                response = (HttpWebResponse)request.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    res = sr.ReadToEnd();
                rootObj = JsonConvert.DeserializeObject<RootObject>(res);
                list.Add(rootObj);
                if (!rootObj.meta.has_next)
                    break;
                adress = adress.Replace("page=" + pageNum, "page=" + (++pageNum));
            }
            return list;
        }
        public static List<Teacher> GetTeacherList(List<RootObject> RootObjectList)
        {
            List<Teacher> list = new List<Teacher>();
            foreach (RootObject root in RootObjectList)
            {
                foreach (Datum item1 in root.data)
                {
                    list.Add(new Teacher()
                    {
                        Name = item1.nickname,
                        Ranking = item1.teacher_info_obj.pro_rating,
                        StudentsNumber = item1.teacher_info_obj.student_count,
                        LessonsNumber = item1.teacher_info_obj.session_count,
                        MinPrice = item1.teacher_info_obj.min_price_usd / 100,
                        MaxPrice = item1.teacher_info_obj.max_price_usd / 100,
                        Languages = item1.language_obj_s,
                        Tags = item1.personal_tag
                    });
                }
            }
            foreach (var item in list)
            {
                foreach (var item1 in item.Tags)
                {
                    item.TagsList.Add(new PersonalTag { TagName = new string(item1.ToString().SkipWhile(i => i != ':').TakeWhile(i => i != ',').ToArray()).Trim(':', ' ', '"') });
                }
            }
            return list;
        }
        public static void Create(List<Teacher> list)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString))
            {
                int tempId;
                string sqlQuery;
                foreach (Teacher t in list)
                {
                    sqlQuery = "INSERT INTO Teachers(Name,Ranking,LessonsNumber,StudentsNumber,MinPrice,MaxPrice) VALUES(@Name,@Ranking,@LessonsNumber,@StudentsNumber,@MinPrice,@MaxPrice); SELECT CAST (SCOPE_IDENTITY() as int)";
                    tempId = db.Query<int>(sqlQuery, t).FirstOrDefault();
                    t.Id = tempId;
                    foreach (LanguageObj l in t.Languages)
                    {
                        sqlQuery = "INSERT INTO LanguageObjs(language,level,priority,is_teaching,is_learning,TeacherId) VALUES(@language,@level,@priority,@is_teaching,@is_learning,@TeacherId)";
                        db.Execute(sqlQuery, new
                        {
                            l.language,
                            l.level,
                            l.priority,
                            l.is_teaching,
                            l.is_learning,
                            TeacherId = t.Id
                        });
                    }
                    foreach (PersonalTag s in t.TagsList)
                    {
                        sqlQuery = "INSERT INTO PersonalTags (TagName,TeacherId) VALUES (@TagName,@TeacherId)";
                        db.Execute(sqlQuery, new
                        {
                            TagName = s.TagName,
                            TeacherId = t.Id
                        });
                    }
                }
            }
        }
        public static List<Teacher> Get()
        {
            List<Teacher> teachers;
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString))
            {
                teachers = (List<Teacher>)db.Query<Teacher>("select * from Teachers");
                Console.WriteLine(teachers.Count);
                string sqlQuery = "select * from LanguageObjs where TeacherId=@Id";
                foreach (var item in teachers)
                {
                    using (var multRes = db.QueryMultiple(sqlQuery, new { Id = item.Id }))
                    {
                        var languages = multRes.Read<LanguageObj>().ToList();
                        if (languages != null)
                            item.Languages.AddRange(languages);
                    }
                }
                sqlQuery = "select * from PersonalTags where TeacherId=@Id";
                foreach (var item in teachers)
                {
                    using (var multRes = db.QueryMultiple(sqlQuery, new { Id = item.Id }))
                    {
                        var tags = multRes.Read<PersonalTag>().ToList();
                        if (tags != null)
                            item.TagsList.AddRange(tags);
                    }
                }
            }
            return teachers;
        }

    }
}