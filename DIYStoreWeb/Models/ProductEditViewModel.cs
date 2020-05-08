using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DIYStoreWeb.Models
{
    public class ProductEditViewModel
    {
        [Display(AutoGenerateField = true)]
        public int ProductId { get; set; }

        [Display(Name = "Имя продутка")]
        public string Name { get; set; }

        [Display(Name = "Производитель")]
        public string Brand { get; set; }

        [Display(Name = "Описание товара")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Единица измерения товара")]
        public int UnitId { get; set; }

        public IEnumerable<SelectListItem> Units { get; set; }
        
        [Display(Name = "Количество в наличии")]
        [Range(1,100)]
        public long Quantity { get; set; }
        
        public string ImageSource { get; set; }

        [Display(Name = "Текущее Изображение")]
        public string ImagePath => $"~/images/{ImageSource}";

        [Display(Name = "Загрузить изображение")]
        public IFormFile ImageFile { get; set; }
    }
}
