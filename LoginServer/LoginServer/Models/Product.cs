using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginServer.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public double Price{ get; set; }
        public int Qty{ get; set; }
    }
}