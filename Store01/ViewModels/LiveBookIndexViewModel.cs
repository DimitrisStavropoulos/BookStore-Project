using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store01.ViewModels
{
    public class LiveBookIndexViewModel
    {
        public string genre { get; set; }
        public string author { get; set; }
        public string publisher { get; set; }
        public string search { get; set; }
        public string sortBy { get; set; }
        public int? page { get; set; }




    }
}