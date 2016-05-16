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
            var users = db.Users.Include(u => u.Roles);
            return View(users.ToList());
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.IdRole = new SelectList(db.Roles, "IdRole", "Role");
            return View();
        }

        // POST: Users/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdUser,FirstName,SecondName,Email,Password,ConfirmPassword,IdRole")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdRole = new SelectList(db.Roles, "IdRole", "Role", users.IdRole);
            return View(users);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Users/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    db.Users.Add(account);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");

                    //ModelState.Clear();
                    // ViewBag.Message = "Здравствуйте "+account.FirstName + " " + account.LastName + " ";
                }
            }

            return View();
        }

        //Login
        public ActionResult Login()
        {
            return View();
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
                        //return RedirectToAction("LoggedIn");
                        return RedirectToAction("Index", "Home");
                    }
                    //return View();
            }
            else
            {
                ModelState.AddModelError("", "Проверьте логин или пароль");
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
