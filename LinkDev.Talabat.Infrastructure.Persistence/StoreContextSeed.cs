using LinkDev.Talabat.Core.Domain.Entities.Products;
using System.Text.Json;

namespace LinkDev.Talabat.Infrastructure.Persistence
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext dbContext)
        {


            if (!dbContext.Brands.Any())
            {


                var filePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..","..", "LinkDev.Talabat.Infrastructure.Persistence", "Data", "DataSeeding", "brands.json");

                var brandsData = await File.ReadAllTextAsync(filePath);
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                if (brands?.Count > 0)
                {

                    await dbContext.Brands.AddRangeAsync(brands);
                    await dbContext.SaveChangesAsync();

                }

            }


            if (!dbContext.Categories.Any())
            {

                var filePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "LinkDev.Talabat.Infrastructure.Persistence", "Data", "DataSeeding", "categories.json");


                var categoriesData = await File.ReadAllTextAsync(filePath);
                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);

                if (categories?.Count > 0)
                {

                    await dbContext.Categories.AddRangeAsync(categories);
                    await dbContext.SaveChangesAsync();

                }

            }


            if (!dbContext.Products.Any())
            {


                var filePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "LinkDev.Talabat.Infrastructure.Persistence", "Data", "DataSeeding", "products.json");


                var productsData = await File.ReadAllTextAsync(filePath);
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if (products?.Count > 0)
                {

                    await dbContext.Products.AddRangeAsync(products);
                    await dbContext.SaveChangesAsync();

                }

            }


        }
    }
}
