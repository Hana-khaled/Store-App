using Microsoft.Extensions.Logging;
using Store.Data.Contexts;
using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Repository
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreDbContext context, ILoggerFactory loggerFactory) 
        {
			try
			{
                // the purpose of this method is add intialized data to database only if it is empty

                // We want to check first if the list is intialized (not null), and intialized doesn't nessecery mean that it has elements
                // then we want to know if it has no element by using any()
                // because applying any()(return true if the list has at least one element, false -> empty)
                // on the list without being intialized will lead to error

                // 1- read data from txt file (File.ReadAllText)
                // 2- deserilize data json -> object (JsonSerializer.Deserialize<Tvale>(json object))
                // 3- add data to db if not null (context.Products.AddRangeAsync(products))
                // 4- after handling all data -> await context.SaveChangesAsync();

                if (context.ProductBrands != null && !context.ProductBrands.Any())
				{
                    
                    var brandsData = File.ReadAllText("../Store.Repository/SeedData/brands.json");
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                    if(brands is not null)
                        await context.ProductBrands.AddRangeAsync(brands);

                }

                if(context.ProductTypes != null && !context.ProductTypes.Any())
                {
                    var typesData = File.ReadAllText("../Store.Repository/SeedData/types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                    if (types is not null)
                        await context.ProductTypes.AddRangeAsync(types);
                }

                if(context.Products != null && !context.Products.Any())
                {
                    var productsData = File.ReadAllText("../Store.Repository/SeedData/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                    if (products is not null)
                        await context.Products.AddRangeAsync(products);
                }

                await context.SaveChangesAsync();
            }
			catch (Exception ex)
			{
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
			}
        }
    }
}
