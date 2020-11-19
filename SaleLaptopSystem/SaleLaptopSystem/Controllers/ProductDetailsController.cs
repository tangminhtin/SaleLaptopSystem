using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SaleLaptopSystem.DAL;
using SaleLaptopSystem.Models;

namespace SaleLaptopSystem.Controllers
{
    public class ProductDetailsController : Controller
    {
        private SaleLaptopSystemContext db = new SaleLaptopSystemContext();

        // GET: ProductDetails
        public ActionResult Index()
        {
            return View(db.ProductDetails.ToList());
        }

        // GET: ProductDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("/");
            }
            ProductDetail productDetail = db.ProductDetails.Find(id);
            if (productDetail == null)
            {
                return RedirectToAction("/Home/Index");
            }

            int proId = (int)id;
            var userComment = from com in db.Comments
                              join usr in db.Users
                              on com.UserID equals usr.ID
                              where com.ProductID == proId && com.Accept == true
                              select new UserComment()
                              {
                                  Fullname = usr.Fullname,
                                  Image = usr.Image,
                                  Content = com.Content,
                                  Date = com.date,
                                  Accept = com.Accept,
                                  Role = usr.Role
                              };
            ViewBag.UserComment = userComment;
            ViewBag.proId = proId;
            return View(productDetail);
        }

        [HttpPost]
        public void AddComment()
        {
            string content = Request["content"];
            string userID = Request["userID"];
            string prodId = Request["productID"];
            Comment comment = new Comment();
            comment.Content = content;
            comment.date = DateTime.Now;
            comment.Accept = true;
            comment.Active = true;
            comment.UserID = Convert.ToInt32(userID);
            comment.ProductID = Convert.ToInt32(prodId);
            db.Comments.Add(comment);
            db.SaveChanges();
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
