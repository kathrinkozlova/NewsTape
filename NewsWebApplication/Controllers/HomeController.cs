using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsWebApplication.Models;

namespace NewsWebApplication.Controllers
{
    public class HomeController : Controller
    {
        NewsTapeDBEntities db = new NewsTapeDBEntities();

        // GET: Home
        public ActionResult Index(int? id)
        {
            var page = id ?? 0;
            if (Request.IsAjaxRequest())
            {
                return PartialView("Index", GetNews(page));
            }

            return View("Index", db.News.OrderByDescending(x => x.Date).Take(5));

        }

        public ActionResult News(int? id)
        {
            var page = id ?? 0;

            if (Request.IsAjaxRequest())
            {
                return PartialView("News", GetNews(page));
            }

            return View("Index", db.News.Take(5));
        }

        private List<News> GetNews(int page = 1)
        {
            var skipRecords = page * 5;

            var listOfNews = db.News;

            return listOfNews.
                OrderByDescending(x => x.Date).
                Skip(skipRecords).
                Take(5).ToList();
        }
    }
}