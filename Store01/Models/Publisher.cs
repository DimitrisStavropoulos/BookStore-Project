using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Store01.Models
{
    public class Publisher
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The publisher name cannot be blank")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please enter a publisher between 3 and 50 characters in length")]
        //[RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Please enter a publisher name beginning with a capital letter and made up of letters and spaces only")]
        [Display(Name = "Publisher Name")]
        public string Name { get; set; }
        public virtual ICollection<Book> Books { get; set; }

    }
}