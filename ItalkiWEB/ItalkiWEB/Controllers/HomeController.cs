using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
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
using ItalkiWEB.Models;
using PagedList.Mvc;
using PagedList;
using System.Linq.Dynamic;

namespace ItalkiWEB.Controllers
{
    
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            List<RootObject> roots = InfoManipulation.GetRootList();
            List<Teacher> teachers = InfoManipulation.GetTeacherList(roots);
            InfoManipulation.Create(teachers);
            List<Teacher> list = new List<Teacher>();
            list = InfoManipulation.Get();
            ViewBag.num = list.Count;
            return View(list[100]);
        }
        public ActionResult ShowTable()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetData(ServerSideParams ssp)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString))
            {
                List<Teacher> list = new List<Teacher>();
                list = InfoManipulation.Get();
                int pageIndex = ssp.page;
                int pageSize = ssp.rows;
                int totalRecords = list.Count;
                var totalPages = (int)Math.Ceiling((float)totalRecords / (float)ssp.rows);
                string sortIndex = null;
                string sortOrder = null;
                switch (ssp.sidx)
                {
                    case "Ranking":
                        sortIndex = "Ranking";
                        break;
                    case "LessonsNumber":
                        sortIndex = "LessonsNumber";
                        break;
                    case "StudentNumber":
                        sortIndex = "StudentNumber";
                        break;
                    case"MinPrice":
                        sortIndex = "MinPrice";
                        break;
                    case "MaxPrice":
                        sortIndex = "MaxPrice";
                        break;
                    default:
                        sortIndex = "Name";
                        break;
                }
                sortOrder = ssp.sord == "asc" ? "asc" : "desc";
                list = db.Query<Teacher>((@"SELECT Name, Ranking, LessonsNumber, StudentsNumber, MinPrice, MaxPrice
                FROM Teachers
                ORDER BY " + sortIndex+" "+sortOrder+ 
                @" OFFSET @rowsToSkip ROWS 
                FETCH NEXT @rowsToTake ROWS ONLY"), new
                    {
                        rowsToSkip = ssp.page * ssp.rows,
                        rowsToTake = pageSize
                    }).ToList();

                if (!string.IsNullOrEmpty(ssp.searchString))
                {
                    list = db.Query<Teacher>((@"SELECT Name, Ranking, LessonsNumber, StudentsNumber, MinPrice, MaxPrice
                    FROM Teachers
                    WHERE MinPrice = @value"), new { value = ssp.searchString }).ToList();
                }
                var jsonData = new
                {
                    total = totalPages,
                    page = pageIndex,
                    records = totalRecords,
                    rows = ssp.rows,
                    root=list
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public string AddNewTeacher( Teacher teacher)
        {
            string msg = null;
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString))
            {
                db.Execute(("INSERT INTO Teachers(Name, Ranking, LessonsNumber, StudentsNumber, MinPrice, MaxPrice) VALUES(@Name, @Ranking, @LessonsNumber, @StudentsNumber, @MinPrice, @MaxPrice"), new
                {
                    Name=teacher.Name,
                    Ranking=teacher.Ranking,
                    LessonsNumber=teacher.LessonsNumber,
                    StudentsNumber=teacher.StudentsNumber,
                    MinPrice=teacher.MinPrice,
                    MaxPrice=teacher.MaxPrice
                });
                msg = "Added Successfully";
            }
            return msg;
        }
    }
}
