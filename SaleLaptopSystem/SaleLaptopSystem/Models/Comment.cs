using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaleLaptopSystem.Models
{
    public class Comment
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public DateTime date { get; set; }
        public bool Accept { get; set; }
        public bool Active { get; set; }

        public int? UserID { get; set; }
        public virtual User User { get; set; }

        public int? ProductID { get; set; }
        public virtual Product Product { get; set; }
    }
}