using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vroom.Models
{
    public class Model   //ovaj model predstavlja vozilo 
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public Make Make { get; set; }  //svako vozilo ima neku marku npr ford 

        public int MakeId { get; set;  }
    }
}
