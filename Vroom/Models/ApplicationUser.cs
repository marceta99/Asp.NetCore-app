using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Vroom.Models
{
    public class ApplicationUser : IdentityUser
    {
        //ovu klasu ApplicationUser sam dodao  jer u IdentitiyUser moze da se zabelezi samo jedan broj
        //telefona a ja zelim da moze 2 i takodje nema ovaj property isAdmin 
        //i kad dodam migraciju onda ce u tabelu AspNetUsers da se doda jos jedna kolona PhoneNumber2
        [DisplayName("Office Phone")]
        public string PhoneNumber2 { get; set; }


        [NotMapped]   //ovo znaci da necu da se ovaj property cuva u bazi podataka tj kada dodam migraciju nece da se napravi jos jedan kolona IsAdmin
        public bool isAdmin { get; set;  }

    }
}
