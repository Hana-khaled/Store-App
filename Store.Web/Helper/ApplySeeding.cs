using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Data.Contexts;
using Store.Data.Entities.IdentityEntities;
using Store.Repository;

namespace Store.Web.Helper
{
    public class ApplySeeding
    {

        /* in order to call SeedAsync: 1- create scope 
         *                             2- to create required objects 
         *                             3- to be passed as arguments for SeedAsync()
         * 
         * We need to pass objects(StoreDbContext, IlLoggerFactory) for static method (seedAsync in class
         * StoreContextSeed)
         * this can be done using dependency injection but in this case we don't have an interface and 
         * concrete class implementing that interface
         * so we use an extra class for making a scope for creating required objects
         * 
         * from WebApplication app -> scope -> ServiceProvider -> GetRequiredService<class type>();
         * call required method
         * 
         * we used using since we are dealing database
         */
        public static async Task ApplySeedingAsync(WebApplication app)
        {
            // We are dealing with database here, thats why we need "using"
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    // Seeding Data
                    var context = services.GetRequiredService<StoreDbContext>();

                    //apply all migrations if not applied first to avoid errors
                    await context.Database.MigrateAsync();

                    await StoreContextSeed.SeedAsync(context, loggerFactory);

                    // Seeding user
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();

                    await StoreIdentityContextSeed.UserSeedAsync(userManager);
                }
                catch (Exception ex)
                {

                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex.Message);
                }
            }
        }
    }
}
