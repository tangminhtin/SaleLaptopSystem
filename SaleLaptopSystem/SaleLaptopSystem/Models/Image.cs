using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaleLaptopSystem.Models
{
    public class Image
    {
        public int ID { get; set; }
        public string image { get; set; }

        public int? ProductID { get; set; }
        public virtual Product Product { get; set; }
    }
}