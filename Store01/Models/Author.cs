using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Store01.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The author's fullname cannot be blank")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please enter a full name between 3 and 50 characters in length")]
        //[RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Please enter a full name beginning with a capital letter and made up of letters and spaces only")]
        [Display(Name = "Author Full Name")]
        public string FullName { get; set; }

        [Display(Name = "About the Author")]
        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        public string AboutInfo { get; set; }

        public virtual ICollection<Book> Books { get; set; }


    }
}