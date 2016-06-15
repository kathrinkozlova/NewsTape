using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NewsWebApplication.Models;

namespace NewsWebApplication.Controllers
{
    public class NewsController : Controller
    {
        private NewsTapeDBEntities db = new NewsTapeDBEntities();

        // GET: News
        public ActionResult Index()
        {
            if (Session["UserName"] != null)
            {
                string userName = Session["UserName"].ToString();

                if (userName != null)
                {
                    var user = db.Users.Where(x => x.Email.Equals(userName)).Select(x => x.IdRole).FirstOrDefault();
                    if (user == 1)
                    {
                        return View(db.News.ToList());
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }


        // GET: News/Create
        public ActionResult Create()
        {
            if (Session["UserName"] != null)
            {
                string userName = Session["UserName"].ToString();

                if (userName != null)
                {
                    var user = db.Users.Where(x => x.Email.Equals(userName)).Select(x => x.IdRole).FirstOrDefault();
                    if (user == 1)
                    {
                        return View();
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: News/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdNews,Title,Image,Text,Date")] News news, HttpPostedFileBase upload)
        {
            if (upload != null && news.Text != null && news.Title != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(upload.FileName);
                // сохраняем файл в папку Files в проекте
                upload.SaveAs(Server.MapPath("~/Images/" + fileName));


                news.Date = DateTime.Now;
                news.Image = fileName;
                if (ModelState.IsValid)
                {
                    db.News.Add(news);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(news);
        }

        // GET: News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserName"] != null)
            {
                string userName = Session["UserName"].ToString();

                if (userName != null)
                {
                    var user = db.Users.Where(x => x.Email.Equals(userName)).Select(x => x.IdRole).FirstOrDefault();
                    if (user == 1)
                    {
                        if (id == null)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }
                        News news = db.News.Find(id);
                        if (news == null)
                        {
                            return HttpNotFound();
                        }
                        return View(news);
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: News/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdNews,Title,Image,Text,Date")] News news, HttpPostedFileBase upload)
        {
            if (upload != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(upload.FileName);
                // сохраняем файл в папку Files в проекте
                upload.SaveAs(Server.MapPath("~/Images/" + fileName));
                news.Image = fileName;
            }
            if (upload == null)
            {
                var image = db.News.Where(x => x.IdNews.Equals(news.IdNews)).Select(x => x.Image).FirstOrDefault();
                if (image != null)
                    news.Image = image;
            }                
            if (news.Image != null && news.Text != null && news.Title != null)
            {
                news.Date = DateTime.Now;
                if (ModelState.IsValid)
                {
                    db.Entry(news).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(news);
        }

        // GET: News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UserName"] != null)
            {
                string userName = Session["UserName"].ToString();

                if (userName != null)
                {
                    var user = db.Users.Where(x => x.Email.Equals(userName)).Select(x => x.IdRole).FirstOrDefault();
                    if (user == 1)
                    {
                        if (id == null)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }
                        News news = db.News.Find(id);
                        if (news == null)
                        {
                            return HttpNotFound();
                        }
                        return View(news);
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            News news = db.News.Find(id);
            db.News.Remove(news);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
