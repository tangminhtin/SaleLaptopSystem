using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaleLaptopSystem.Models
{
    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}