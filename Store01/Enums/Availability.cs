using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Store01.Enums
{
    public enum Availability
    {
        [Display(Name ="In Stock")]
        In_Stock,
        [Display(Name ="Temporarily Unavailable")]
        Temporarily_Unavailable,
        [Display(Name ="No longer available")]
        Unavailable
    }

    

    



}