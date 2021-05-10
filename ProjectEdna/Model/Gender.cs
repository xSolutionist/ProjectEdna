using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectEdna.Model
{
    public enum Gender
    {
        [Display(Name = "M")]
        M = 1,
        [Display(Name = "F")]
        F = 2,

        [Display(Name = "N/A")]
        NA = 3
        
    }
}
