using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaleLaptopSystem.Models
{
    public class ProductDetail
    {
        public int ID { get; set; }
        public string Processor { get; set; }
        public string RAM { get; set; }
        public string Screen { get; set; }
        public string Storage { get; set; }
        public string Graphic { get; set; }
        public string Size { get; set; }
        public string OS { get; set; }
        public string Video { get; set; }
        public string Connection { get; set; }
        public string Keyboard { get; set; }
        public string Battery { get; set; }
        public virtual ICollection<Product> Products { get; set; }

    }
}