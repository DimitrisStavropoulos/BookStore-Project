using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Store01.Models
{
    public class Order
    {

        [Display(Name = "Order ID")]
        public int OrderID { get; set; }
        [Display(Name = "User")]
        public string UserID { get; set; }
        [Display(Name = "Deliver to")]
        public string DeliveryName { get; set; }
        [Display(Name = "Delivery Address")]
        public Address DeliveryAddress { get; set; }
        [Display(Name = "Sum Total")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:n} €")]
        public decimal TotalPrice { get; set; }
        [Display(Name = "Time of Order")]
        public DateTime DateCreated { get; set; }
        public List<OrderLine> OrderLines { get; set; }


    }
}