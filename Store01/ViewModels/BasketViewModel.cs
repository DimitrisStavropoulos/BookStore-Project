using Store01.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Store01.ViewModels
{
    public class BasketViewModel
    {
        public List<BasketLine> BasketLines { get; set; }
        [Display(Name = "Cart Total:")]
        [DisplayFormat(DataFormatString = "{0:n} €")]
        public decimal TotalCost { get; set; } 
    }
} 