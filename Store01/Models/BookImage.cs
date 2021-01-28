using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Store01.Models
{
    public class BookImage
    {
        public int Id { get; set; }

        [Display(Name = "File")]
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string FileName { get; set; }

        public virtual ICollection<BookImageMapping> BookImageMappings { get; set; }


    }
}