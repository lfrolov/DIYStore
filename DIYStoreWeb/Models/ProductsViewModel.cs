using System.Collections.Generic;

namespace DIYStoreWeb.Models
{
    public class ProductsViewModel : IPaginationModel
    {
        public IEnumerable<ProductViewModel> Products { get; set; }
        public PageViewModel Pager  { get; set; }
    }    
}
