using SaleLaptopSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SaleLaptopSystem.DAL
{
    public class SaleLaptopSystemContext: DbContext
    {
        public SaleLaptopSystemContext(): base("SaleLaptopSystem")
        {
            Database.SetInitializer<SaleLaptopSystemContext>(new DropCreateDatabaseIfModelChanges<SaleLaptopSystemContext>());
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}