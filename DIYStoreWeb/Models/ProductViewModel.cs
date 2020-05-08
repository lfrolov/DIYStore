using DIYDb.Models;
using System.ComponentModel.DataAnnotations;

namespace DIYStoreWeb.Models
{
    public class ProductViewModel
    {
        [Display(Name = "Код товара")]
        public int ProductId { get; set; }

        [Display(Name = "Имя продутка")]
        public string Name { get; set; }

        [Display(Name = "Производитель")]
        public string Brand { get; set; }

        public string ImageSource { get; set; }

        [Display(Name = "Изображение")]
        public string ImagePath => $"~/images/{ImageSource}";

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Количество")]
        public long Quantity { get; set; }
        
        [Display(Name = "Единица измерения")]
        public string ShortUnitName { get; set; }

        [Display(Name =  "Количество в наличии")]
        public string UnitQuantity => $"{Quantity} {ShortUnitName}";
    }

    public static class ProductViewModelExtension 
    {
        //ToDo: change to automapper
        public static ProductViewModel ToVm(this Product p)
        {
            return new ProductViewModel
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Brand = p.Brand,
                Description = p.Description,
                ImageSource = p.ImageSource,
                Quantity = p.Quantity,
                ShortUnitName = p.Unit.ShortName
            };
        }
    }
}
