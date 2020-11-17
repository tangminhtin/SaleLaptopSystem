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
using System.Web.Security;
using Facebook;
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
                return Redirect("/");
            } else
            {
                ViewBag.Error = "Your username or password incorrect!";
            }
            return View();
        }
        public ActionResult Logingg()
        {
           string name = Request.Params["name"];
            string picture = Request.Params["picture"];
            string email = Request.Params["email"];
            User us = new User();
            if (db.Users.FirstOrDefault(u => u.Email.Equals(email) && u.Address.Equals("Google")) == null)
            {
                us.Password = MD5Hash(us.Email + "Google");
                us.Role = "user";
                us.Active = true;
                us.Phone = "8888889";
                us.Fullname = name;
                us.Image = picture;
                us.Address = "Google";
                us.Email = email;
                db.Users.Add(us);
                db.SaveChanges();
                Session["User"] = us;

            }
            else
            {
                var users = db.Users;
                var usr = users.FirstOrDefault(u => u.Email.Equals(email) && u.Address.Equals("Google"));
                Session["User"] = usr;
            } 
            return RedirectToAction("Index","Home");
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
        private Uri RediredtUri

        {

            get

            {

                var uriBuilder = new UriBuilder(Request.Url);

                uriBuilder.Query = null;

                uriBuilder.Fragment = null;

                uriBuilder.Path = Url.Action("FacebookCallback");

                return uriBuilder.Uri;

            }

        }















        [AllowAnonymous]

        public ActionResult Facebook()

        {

            var fb = new FacebookClient();




            var loginUrl = fb.GetLoginUrl(new

            {




                client_id = "2624477971146310",

                client_secret = "7fdcecdbff488032054414bb8073c9a4",

                redirect_uri = RediredtUri.AbsoluteUri,

                response_type = "code",

                scope = "email"



            });

            return Redirect(loginUrl.AbsoluteUri);

        }




        public ActionResult FacebookCallback(string code)

        {

            var fb = new FacebookClient();

            dynamic result = fb.Post("oauth/access_token", new

            {

                client_id = "2624477971146310",

                client_secret = "7fdcecdbff488032054414bb8073c9a4",

                redirect_uri = RediredtUri.AbsoluteUri,

                code = code




            });

            var accessToken = result.access_token;

            Session["AccessToken"] = accessToken;

            fb.AccessToken = accessToken;

            dynamic me = fb.Get("me?fields=link,first_name,currency,last_name,email,gender,locale,timezone,verified,picture,age_range");

            //string email = me.email;
            string name = me.first_name+ me.last_name;
            string picture = me.picture.data.url;
            string email = me.email;
            User us = new User();
            if (db.Users.FirstOrDefault(u => u.Email.Equals(email)&& u.Address.Equals("Facebook")) == null)
                {
                    us.Password = MD5Hash(us.Email+"FaceBook");
                    us.Role = "user";
                    us.Active = true;
                    us.Phone = "8888889";
                    us.Fullname = name;
                    us.Image = picture;
                    us.Address = "Facebook";
                    us.Email = email;
                    db.Users.Add(us);
                    db.SaveChanges();
                    Session["User"] = us;
                 
            } else
                {       
                 var users = db.Users;
                var usr = users.FirstOrDefault(u => u.Email.Equals(email) && u.Address.Equals("Facebook"));
                 Session["User"] = usr;
            }

            return RedirectToAction("Index", "Home");


            //FormsAuthentication.SetAuthCookie(email, false);

            //return RedirectToAction("Index", "Home");

        }

    }
}
