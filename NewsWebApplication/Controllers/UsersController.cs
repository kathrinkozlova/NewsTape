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
    public class UsersController : Controller
    {
        private NewsTapeDBEntities db = new NewsTapeDBEntities();

        // GET: Users
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
                        var users = db.Users.Include(u => u.Roles);
                        return View(users.ToList());
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }


        // GET: Users/Edit/5
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
                        Users users = db.Users.Find(id);
                        if (users == null)
                        {
                            return HttpNotFound();
                        }
                        ViewBag.IdRole = new SelectList(db.Roles, "IdRole", "Role", users.IdRole);
                        return View(users);
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdUser,FirstName,SecondName,Email,Password,ConfirmPassword,IdRole")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdRole = new SelectList(db.Roles, "IdRole", "Role", users.IdRole);
            return View(users);
        }

        // GET: Users/Delete/5
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
                        Users users = db.Users.Find(id);
                        if (users == null)
                        {
                            return HttpNotFound();
                        }
                        return View(users);
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Users account)
        {
            var user = db.Users.Any(x => string.Compare(account.Email, x.Email) == 0);
            if (user)
                ModelState.AddModelError("Email", "Пользователь с таким email уже зарегистрирован");
            else {
                account.IdRole = 2;
                if (ModelState.IsValid)
                {
                    Session["UserName"] = account.Email;
                    db.Users.Add(account);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

        //Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public ActionResult IsLoggedIn()
        {
            if (Session["UserName"] != null)
            {
                string userName = Session["UserName"].ToString();

                if (userName != null)
                {
                    var user = db.Users.Where(x => x.Email.Equals(userName)).Select(x => x.IdRole).FirstOrDefault();
                    if (user == 1)
                        return RedirectToAction("Index", "News");
                    if (user == 2)
                        return RedirectToAction("UserSettings", "Users");
                }
            }
            return RedirectToAction("Login");

        }

        [HttpPost]
        public ActionResult Login(Users user)
        {

            if (user.Email != null && user.Password != null)
            {
                var userEmail = db.Users.Any(x => string.Compare(x.Email, user.Email) == 0);
                var userPassword = db.Users.Any(x => string.Compare(x.Password, user.Password) == 0);


                if (userEmail && userPassword)
                {
                    Session["UserId"] = user.IdUser.ToString();
                    Session["UserName"] = user.Email.ToString();
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Проверьте логин или пароль");
                return View();
            }
            return View();

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
