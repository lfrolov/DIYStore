using DIYDb.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace DIYDb 
{
    public partial class DIYDbContext
    {
        protected Random Randomizer { get; } = new Random();
        protected void IniDbData(ModelBuilder modelBuilder)
        {
            InitUnits(modelBuilder);
            InitProducts(modelBuilder);
            //InitCategories(modelBuilder);
        }

        private void InitUnits(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Unit>().HasData(new[] {
                new Unit { UnitId = 1, ShortName = "шт", Description = "штуки" },
                new Unit { UnitId = 2, ShortName = "кг", Description = "килограмм" },
                new Unit { UnitId = 3, ShortName = "л", Description = "литры" },
                new Unit { UnitId = 4, ShortName = "м2", Description = "квадратный метр" },
                new Unit { UnitId = 5, ShortName = "уп", Description = "упаковка" }
            });
        }


        private readonly string[] SawBrands = new[] { "Bosch", "Skill", "Hitachi", "Makita" };
        private readonly string[] EScrewdriverBrands = new[] { "Bosch", "Hitachi", "Makita", "Patriot", "Вихрь" };
        private readonly string[] TitleBrands = new[] { "Cersanit", "LB", "Kerama Marazi", "Azori" };

        private void InitProducts(ModelBuilder modelBuilder)
        {
            Tuple<string, string[], string, Range, int>[] ProductTypes = new[]
            {
                new Tuple<string, string[], string, Range, int>("Дисковая пила", SawBrands, "Saw1.jpg", 1000..9000, 1),
                new Tuple<string, string[], string, Range, int>("Аккумуляторный шуруповерт", EScrewdriverBrands, "Screw1.jpg", 1000..9000, 1),
                new Tuple<string, string[], string, Range, int>("Плитка", TitleBrands, "Title1.jpg", 1..10, 5)
            };

            var products = new Product[47];
            for (int i = 0; i < products.Length; i++)
            {
                var productType = ProductTypes[Randomizer.Next(ProductTypes.Length)];
                string brand = productType.Item2[Randomizer.Next(productType.Item2.Length)];
                products[i] = new Product
                {
                    ProductId = i + 1,
                    Name = $"{productType.Item1} {brand} {Randomizer.Next(productType.Item4.Start.Value, productType.Item4.End.Value)}",
                    Brand = brand,
                    ImageSource = productType.Item3,
                    UnitId = productType.Item5,
                    Quantity = Randomizer.Next(100)
                };
            }

            modelBuilder.Entity<Product>(
                entity => 
                {
                    entity.HasKey("ProductId");
                    entity.Property(p => p.ProductId).UseIdentityColumn();
                    
                    entity.HasData(products);
                });
        }
    }
}