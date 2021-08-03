using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vroom.Models.ViewModel
{
    public class BikeViewModel
    {
        public Bike Bike { get; set; }

        public IEnumerable<Make> Makes { get; set; }
        public IEnumerable<Model> Models { get; set; }

        public IEnumerable<Currency> Currencies { get; set;  }

        public List<Currency> CList = new List<Currency>(); 

        public List<Currency> CreateList()
        {
            CList.Add(new Currency("USD", "USD")); 
            CList.Add(new Currency("EUR", "EUR")); 
            CList.Add(new Currency("DIN", "DIN"));

            return CList; 
        }

        public BikeViewModel()
        {
            Currencies = CList; 
        }


    }

    public class Currency
    {
        public string Id { get; set; }
        public string Name { get; set;  }

        public Currency(string id, string name)
        {
            Id = id;
            Name = name; 

        } 


    }
}
