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
using PagedList.Mvc;
using PagedList;

namespace SaleLaptopSystem.Controllers
{
    public class ProductsController : Controller
    {
        private SaleLaptopSystemContext db = new SaleLaptopSystemContext();

        public ActionResult Apple(int brand)
        {

            var products = db.Products.Include(p => p.Brand).Include(p => p.Category).Include(p => p.ProductDetail);
            products = products.Where(p => p.BrandID == 1);
           
            return View(products.ToList());
        }


        // GET: Products
        public ActionResult Index(int? brand, double? maximumPrice, double? minimunPrice, int? page, string sortOrder)
        {
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            ViewBag.CurrentSort = sortOrder;
            var products = db.Products.Include(p => p.Brand).Include(p => p.Category).Include(p => p.ProductDetail);
            //.Include(p => p.Images)

            if (!string.IsNullOrEmpty(brand.ToString()))
            {
                String[] brands = {"Asus", "Dell", "Apple", "HP" };
                ViewBag.Brand = brands[brand.Value - 1];
                products = products.Where(p => p.BrandID == brand);
                // .Include(p => p.Images)
            }
            else
            {
                products = products.Where(p => p.Price >= minimunPrice.Value && p.Price <= maximumPrice.Value);
            }
            return View(products.ToList().ToPagedList(pageNumber, pageSize));
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
