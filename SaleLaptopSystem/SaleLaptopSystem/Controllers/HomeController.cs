using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using SaleLaptopSystem.DAL;
using SaleLaptopSystem.Models;
using PagedList.Mvc;
namespace SaleLaptopSystem.Controllers
{
    public class HomeController : Controller
    {
        private SaleLaptopSystemContext db = new SaleLaptopSystemContext();
        public ActionResult Index(int? page, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            ViewBag.Home = db.Products.ToList().ToPagedList(pageNumber , pageSize);
            ViewBag.Apple = db.Products.Where(x => x.BrandID == 3).ToList().ToPagedList(pageNumber, pageSize);
            ViewBag.Asus = db.Products.Where(x => x.BrandID == 1).ToList().ToPagedList(pageNumber, pageSize);
            ViewBag.Dell = db.Products.Where(x => x.BrandID == 2).ToList().ToPagedList(pageNumber, pageSize);
            ViewBag.HP = db.Products.Where(x => x.BrandID == 4).ToList().ToPagedList(pageNumber, pageSize);
            return View(ViewBag.Home);
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}