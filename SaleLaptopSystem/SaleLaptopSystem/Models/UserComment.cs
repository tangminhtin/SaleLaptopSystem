using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaleLaptopSystem.Models
{
    public class UserComment
    {
        public string Fullname { get; set; }
        public string Image { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public bool Accept { get; set; }
        public string Role { get; set; }
    }
}