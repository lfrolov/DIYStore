using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DIYDb.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public string Brand { get; set; }

        public string Description { get; set; }

        public int UnitId { get; set; }

        public Unit Unit {get; set;}

        public long Quantity { get; set; }
        [Display(Name = "Изображение")]
        public string ImageSource { get; set; }
        
    }
}
