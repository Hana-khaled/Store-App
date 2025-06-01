using Microsoft.AspNetCore.Identity;
using Store.Data.Contexts;
using Store.Data.Entities.IdentityEntities;
using System.Net.WebSockets;

namespace Store.Web.Extensions
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentitySerices(this IServiceCollection services)
        {
            // 1- Registering the identity core in our application for AppUser
            var builder = services.AddIdentityCore<AppUser>();

            // 2- Intializing IdentityBuilder
            builder = new IdentityBuilder(builder.UserType, builder.Services);

            // 3- Adding entityframeworkstores for StoreIdentityDbContext that manage user
            builder.AddEntityFrameworkStores<StoreIdentityDbContext>();

            // 4- Adding SignInManager
            builder.AddSignInManager<SignInManager<AppUser>>();

            services.AddAuthentication();

            return services;
        }
    }
}
