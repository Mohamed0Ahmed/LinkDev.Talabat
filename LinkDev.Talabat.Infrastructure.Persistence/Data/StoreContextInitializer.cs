using LinkDev.Talabat.Core.Domain.Contracts;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data
{
    internal class StoreContextInitializer : IStoreContextInitializer
    {
        private readonly StoreContext _dbContext;

        public StoreContextInitializer(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }






        public async Task InitializeAsync()
        {
            var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
                await _dbContext.Database.MigrateAsync();    // Update-Database
        }


        public async Task SeedAsync()
        {
            if (!_dbContext.Brands.Any())
            {


                var filePath = Path.Combine(AppContext.BaseDirectory, "../../../..", "LinkDev.Talabat.Infrastructure.Persistence", "Data", "DataSeeding", "brands.json");

                var brandsData = await File.ReadAllTextAsync(filePath);
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                if (brands?.Count > 0)
                {

                    await _dbContext.Brands.AddRangeAsync(brands);
                    await _dbContext.SaveChangesAsync();

                }

            }


            if (!_dbContext.Categories.Any())
            {

                var filePath = Path.Combine(AppContext.BaseDirectory, "../../../..", "LinkDev.Talabat.Infrastructure.Persistence", "Data", "DataSeeding", "categories.json");


                var categoriesData = await File.ReadAllTextAsync(filePath);
                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);

                if (categories?.Count > 0)
                {

                    await _dbContext.Categories.AddRangeAsync(categories);
                    await _dbContext.SaveChangesAsync();

                }

            }


            if (!_dbContext.Products.Any())
            {


                var filePath = Path.Combine(AppContext.BaseDirectory, "../../../..", "LinkDev.Talabat.Infrastructure.Persistence", "Data", "DataSeeding", "products.json");


                var productsData = await File.ReadAllTextAsync(filePath);
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if (products?.Count > 0)
                {

                    await _dbContext.Products.AddRangeAsync(products);
                    await _dbContext.SaveChangesAsync();

                }

            }

        }


    }
}
