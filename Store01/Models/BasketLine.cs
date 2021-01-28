using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Store01.Models
{
    public class BasketLine
    {
        public int ID { get; set; }
        public string BasketID { get; set; }
        public int BookID { get; set; }
        [Range(0, 50, ErrorMessage = "Please enter a quantity between 0 and 50")]
        public int Quantity { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual Book Book { get; set; }
    }
}