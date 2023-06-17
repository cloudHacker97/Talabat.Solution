using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data
{
	public static class StoreContextSeed
	{

		public static async Task SeedAsync(StoreContext Dbcontext)
		{
			
				var BrandData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
				var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandData);

				if (Brands?.Count > 0)
				{
					foreach (var brand in Brands)
					{
						await Dbcontext.Set<ProductBrand>().AddAsync(brand);
					}
				await Dbcontext.SaveChangesAsync();
			}
			

				var TypeData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
				var Types = JsonSerializer.Deserialize<List<ProductType>>(TypeData);

				if (Types?.Count > 0)
				{
					foreach (var type in Types)
					{
						await Dbcontext.Set<ProductType>().AddAsync(type);
					}
				await Dbcontext.SaveChangesAsync();
			}	
				var ProductData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
				var Products = JsonSerializer.Deserialize<List<Product>>(ProductData);

				if (Products?.Count > 0)
				{
					foreach (var product in Products)
					{
						await Dbcontext.Set<Product>().AddAsync(product);
					}
					await Dbcontext.SaveChangesAsync();
				}
		}
	}
}
