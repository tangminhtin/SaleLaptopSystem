using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaleLaptopSystem.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public string Description { get; set; }
        public string Features { get; set; }
        public bool Active { get; set; }

        public int? BrandID { get; set; }
        public virtual Brand Brand { get; set; }

        public int? CategoryID { get; set; }
        public virtual Category Category { get; set; }

        public int? ProductDetailID { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
    
}