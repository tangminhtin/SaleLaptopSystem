using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            //user.Password = MD5Hash(user.Password);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit()
        {
            int id = Convert.ToInt32(Request["id"]);
            string name = Request["name"];
            string phone = Request["phone"];
            string pass = Request["pass"];
            string confirm = Request["confirm"];
            string address = Request["address"];
            User u = db.Users.FirstOrDefault(x => x.ID == id);
            u.Fullname = name;
            u.Phone = phone;
            u.Password = MD5Hash(pass);
            u.Address = address;

            db.Entry(u).State = EntityState.Modified;
            db.SaveChanges();
            Session["update"] = "Update Successfull!";
            //return Redirect("/");
            return View(u);
        }

        [HttpPost]
        public ActionResult Reset(int? id)
        {
            if (Session["code"] == null)
            {
                string email = Request["email"];
                Random r = new Random();
                int rand = r.Next(100000, 999999);
                SendMail(email, "Reset your password!", "This verification code was sent to your email for help getting back into account: " + rand);
                Session["code"] = rand;
                Session["email"] = email;

            }
            else
            {
                string code = Session["code"].ToString();
                string codeIn = Request["code"];
                string pass = Request["pass"];
                string confirm = Request["confirm"];
                string email = Session["email"].ToString();
                if (code.Equals(codeIn))
                {
                    User u = db.Users.FirstOrDefault(x => x.Email.Equals(email));
                    u.Password = MD5Hash(pass);
                    db.Entry(u).State = EntityState.Modified;
                    db.SaveChanges();
                    Session["code"] = null;
                    Session["email"] = null;
                    return Redirect("/Users/Login");
                }
            }


            return View();
        }

        public ActionResult Reset()
        {

            return View();
        }
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
                Session["User"] = usr;
                return Redirect("/");
            }
            else
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
            return RedirectToAction("Index", "Home");
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
                }
                else
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
            string name = me.first_name + me.last_name;
            string picture = me.picture.data.url;
            string email = me.email;
            User us = new User();
            if (db.Users.FirstOrDefault(u => u.Email.Equals(email) && u.Address.Equals("Facebook")) == null)
            {
                us.Password = MD5Hash(us.Email + "FaceBook");
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

            }
            else
            {
                var users = db.Users;
                var usr = users.FirstOrDefault(u => u.Email.Equals(email) && u.Address.Equals("Facebook"));
                Session["User"] = usr;
            }
            return RedirectToAction("Index", "Home");
        }

        public string SendMail(string sendto, string subject, string content)
        {
            string _from = "doubleTShop3311@gmail.com";
            string _pass = "Doubletshop123";
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(_from);
                mail.To.Add(sendto);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = content;
                mail.Priority = MailPriority.High;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(_from, _pass);
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

    }
}
