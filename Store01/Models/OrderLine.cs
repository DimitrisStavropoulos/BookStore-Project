using System.ComponentModel.DataAnnotations;

namespace Store01.Models
{
    public class OrderLine
    {

        public int ID { get; set; }
        public int OrderID { get; set; }
        public int? BookId { get; set; }
        public int Quantity { get; set; }
        public string BookTitle { get; set; }
        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:n} €")]
        public decimal UnitPrice { get; set; }
        public virtual Book Book { get; set; }
        public virtual Order Order { get; set; }


    }
}