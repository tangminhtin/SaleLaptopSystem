using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaleLaptopSystem.Models
{
    public class Brand
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}