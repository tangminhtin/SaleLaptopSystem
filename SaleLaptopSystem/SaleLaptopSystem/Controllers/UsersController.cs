using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using SaleLaptopSystem.DAL;
using SaleLaptopSystem.Models;

namespace SaleLaptopSystem.Controllers
{
    public class UsersController : Controller
    {
        private SaleLaptopSystemContext db = new SaleLaptopSystemContext();

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Email,Password")] User user)
        {
            var users = db.Users;
            string pass = MD5Hash(user.Password);
            var usr = users.FirstOrDefault(u => u.Email.Equals(user.Email) && u.Password.Equals(pass));
            if (usr != null)
            {
                Session["User"] = usr ;
                return RedirectToAction("Index");
            } else
            {
                ViewBag.Error = "Your username or password incorrect!";
            }
            return View();
        }
        public ActionResult SignUp()
        {
            return View();
        } 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp([Bind(Include = "ID,Fullname,Password,Email,Phone,Address,Image,Role,Active")] User user)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.FirstOrDefault(u => u.Email.Equals(user.Email)) == null)
                {
                    user.Password = MD5Hash(user.Password);
                    user.Role = "user";
                    user.Active = true;
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Login");
                } else
                {
                    ViewBag.Error = "Your account has already";
                }
            }
            return View(user);
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            return Redirect("/");
        }

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,Fullname,Password,Email,Phone,Address,Image,Role,Active")] User user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Users.Add(user);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(user);
        //}

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Fullname,Password,Email,Phone,Address,Image,Role,Active")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
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
