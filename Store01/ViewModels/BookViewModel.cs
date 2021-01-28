using Store01.Enums;
using Store01.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Store01.ViewModels
{
    public class BookViewModel
    {
        
            //[Required]
            public int Id { get; set; }
            //esoterikos kwdikos
            [Required(ErrorMessage = "Book Code cannot be blank")]
            [StringLength(10, MinimumLength = 1)]

            public string Code { get; set; }

            [Required(ErrorMessage = "The Book title cannot be blank")]
            [StringLength(100, MinimumLength = 1)]
            public string Title { get; set; }


            [Required(ErrorMessage = "The Book description cannot be blank")]
            [StringLength(500, ErrorMessage = "Book description cannot exceed 500 characters")]
            [DataType(DataType.MultilineText)]
            public string Description { get; set; }

            [Required(ErrorMessage = "ISBN cannot be blank")]
            //[RegularExpression("[0-9]{3}[-][0-9]{10}", ErrorMessage = "Isbn must be of the form NNN-NNNNNNNNNN")]
            [StringLength(14, MinimumLength = 14, ErrorMessage = "ISBN must be exactly 14 characters long")]
            public string Isbn { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            [Display(Name = "Release Date")]
            public DateTime ReleaseDate { get; set; }

            [Range(50, 1700, ErrorMessage = "Book pages must be between 50 and 1700 pages")]
            public int Pages { get; set; }

            public int AuthorId { get; set; }
            public virtual Author Author { get; set; }

            public int PublisherId { get; set; }
            public virtual Publisher Publisher { get; set; }

            public int GenreId { get; set; }
            public virtual Genre Genre { get; set; }

            public int ImageId { get; set; }
            public virtual BookImage BookImage { get; set; }

            [Required(ErrorMessage = "The price cannot be blank")]
            [DataType(DataType.Currency)]
            [DisplayFormat(DataFormatString = "{0:n} €")]
            [RegularExpression("[0-9]+(\\.[0-9][0-9]?)?", ErrorMessage = "The price must be a number up to two decimal places")]
            public decimal Price { get; set; }

            public Availability Availability { get; set; }

            public SelectList GenreList { get; set; }
            public SelectList AuthorList { get; set; }
            public SelectList PublisherList { get; set; }
            public List<SelectList> ImageLists { get; set; }
            public string[] BookImages { get; set; }





        





    }
}