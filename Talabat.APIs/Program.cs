using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.APIs.MiddleWares;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddSwaggerService();

			builder.Services.AddDbContext<StoreContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddApplicationService();


			var app = builder.Build();

            using var scope=app.Services.CreateScope();
            var services=scope.ServiceProvider;
            var LoggerFactory=services.GetRequiredService<ILoggerFactory>();
            try
            {
				var dbContext = services.GetRequiredService<StoreContext>();
				await dbContext.Database.MigrateAsync();
				await StoreContextSeed.SeedAsync(dbContext);
			}
            catch (Exception ex)
            {
                var logger=LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error Occured During Applying the Migrations");
            }


            //StoreContext dbContext = new StoreContext();


            // Configure the HTTP request pipeline.
            app.UseMiddleware<ExceptionMiddleware>();
			if (app.Environment.IsDevelopment())
            {
               app.UseSwaggerMiddleWare();
            }
            app.UseStatusCodePagesWithRedirects("/errors/{0}");
            app.UseHttpsRedirection();

           // app.UseStaticFiles();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}