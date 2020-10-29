using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaleLaptopSystem.Models
{   
    public class Order
    {
        public int ID { get; set; }
        public DateTime date { get; set; }
        public int TotalQuantiy { get; set; }
        public double TotalPrice { get; set; }
        public string Note { get; set; }
        public bool Active { get; set; }

        public int? UserID { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}