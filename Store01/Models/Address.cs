using System.ComponentModel.DataAnnotations;

namespace Store01.Models
{
    public class Address
    {
        //Address is a distinct class because it will be used for Orders as well as Users.

        [Required]
        [Display(Name = "Address Line ")]
        public string AddressLine { get; set; }
        //[Display(Name = "Address Line 2")]
        //public string AddressLine2 { get; set; }
        [Required]
        public string City { get; set; }
        //[Required]
        //public string County { get; set; }
        [Required]
        public string Postcode { get; set; }






    }
}