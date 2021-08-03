using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vroom.Models
{
    public class Make //make je marka vozila , znaci npr ducati je marka , ferrari je marka 
    {
        public int Id { get; set; } 
        
        [Required]
        [StringLength(255)]
        public string Name { get; set;  }
    }
}
