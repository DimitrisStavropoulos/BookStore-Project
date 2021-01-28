using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Store01.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Genre title cannot be blank")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please enter a genre between 3 and 50 characters in length")]
        //[RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = "Please enter a genre title beginning with a capital letter and made up of letters and spaces only")]
        [Display(Name = "Genre Title")]
        public string Title { get; set; }

        public virtual ICollection<Book> Books { get; set; }




    }
}