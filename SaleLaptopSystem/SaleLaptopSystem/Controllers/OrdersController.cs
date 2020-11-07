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
    public class OrdersController : Controller
    {
        private SaleLaptopSystemContext db = new SaleLaptopSystemContext();

        public ActionResult Checkout()
        {
            if (Session["User"] == null)
            {
                return Redirect("/Users/Login");
            }
            return View();
        }

        public ActionResult Checkout1()
        {
            List<CartItem> cart1 = (List<CartItem>)Session["cart"];
            User us = (User)Session["user"];
            Order od = new Order();
            od.UserID = us.ID;
            od.TotalQuantiy = cart1.Count;
            od.TotalPrice = (Double)cart1.Sum(item => item.Product.Price * item.Quantity);
            od.Active = true;
            DateTime now = DateTime.Now;
            od.date = now;
            od.Note = "đã đặt";
            db.Orders.Add(od);
            db.SaveChanges();
            int oddID = od.ID;
            cart1.ForEach(p =>
            {
                OrderDetail odt = new OrderDetail();
                odt.OrderID = oddID;
                odt.ProductID = p.Product.ID;
                odt.Quantity = p.Quantity;
                odt.Price = p.Product.Price;
                db.OrderDetails.Add(odt);
                db.SaveChanges();
            });
            TempData["AlertMessage"] = "Checkout Successfully!";
            Session.Remove("cart");
            return Redirect("/");
        }
        public ActionResult Cart(int? prodID, string msg)
        {
            if (Session["cart"] == null)
            {
                List<CartItem> carts = new List<CartItem>();
                var products = db.Products.Include(i => i.Images);
                Product product = products.FirstOrDefault(p => p.ID == prodID);
                carts.Add(new CartItem { Product = product, Quantity = 1 });
                Session["cart"] = carts;
            } else
            {
                List<CartItem> carts = (List<CartItem>)Session["cart"];
                int indexExist = carts.FindIndex(c => c.Product.ID == prodID);
                if (indexExist != -1)
                {
                    if (msg != null)
                    {
                        if (msg.Equals("minus"))
                        {
                            carts[indexExist].Quantity--;
                            if(carts[indexExist].Quantity == 0)
                            {
                                carts.RemoveAt(indexExist);
                            }
                        }
                        else if (msg.Equals("add"))
                        {
                            carts[indexExist].Quantity++;
                        }
                    }
                } else
                {
                    var products = db.Products.Include(i => i.Images);
                    Product product = products.FirstOrDefault(p => p.ID == prodID);
                    carts.Add(new CartItem { Product = product, Quantity = 1 });
                }
                Session["cart"] = carts;
            }

            return PartialView();
        }

        public ActionResult Remove(int? prodID)
        {
            List<CartItem> carts = (List<CartItem>)Session["cart"];
            carts.RemoveAt(carts.FindIndex(c => c.Product.ID == prodID));
            Session["cart"] = carts;
            return RedirectToAction("Cart");
        }

        //private int isExist(int prodID)
        //{
        //    List<CartItem> carts = (List<CartItem>)Session["cart"];
        //    return carts.FindIndex(c => c.Product.ID.Equals(prodID));
        //}

        // GET: Orders
        public ActionResult Index()
        {
            //var orders = db.Orders.Include(o => o.User);
            return View();
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.Users, "ID", "Fullname");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,date,TotalQuantiy,TotalPrice,Note,Active,UserID")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.Users, "ID", "Fullname", order.UserID);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "ID", "Fullname", order.UserID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,date,TotalQuantiy,TotalPrice,Note,Active,UserID")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "ID", "Fullname", order.UserID);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
