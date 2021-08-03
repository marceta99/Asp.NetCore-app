using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vroom.Models.ViewModel
{
    public class ModelViewModel
    {
        //public Model Model { get; set;  }

        public IEnumerable<Make> Makes { get; set; }

        //public Make Make { get; set;  }

        public int Id { get; set; }  //makeId

        public int ModelId { get; set;  }

        [Required]
        public string Name { get; set;  }
    }
}
