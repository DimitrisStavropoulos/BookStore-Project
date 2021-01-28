using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Store01.Models
{
    public class BookImageMapping
    {
        public int ID { get; set; }
        public int ImageNumber { get; set; }
        public int BookId { get; set; }
        public int BookImageId { get; set; }
        public virtual Book Book { get; set; }
        public virtual BookImage BookImage { get; set; }



    }
}