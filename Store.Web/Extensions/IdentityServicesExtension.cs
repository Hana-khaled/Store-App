using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Store.Data.Contexts;
using Store.Data.Entities.IdentityEntities;
using System.Net.WebSockets;
using System.Text;

namespace Store.Web.Extensions
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration _config)
        {
            // 1- Registering the identity core in our application for AppUser
            var builder = services.AddIdentityCore<AppUser>();

            // 2- Intializing IdentityBuilder
            builder = new IdentityBuilder(builder.UserType, builder.Services);

            // 3- Adding entityframeworkstores for StoreIdentityDbContext that manage user
            builder.AddEntityFrameworkStores<StoreIdentityDbContext>();

            // 4- Adding SignInManager
            builder.AddSignInManager<SignInManager<AppUser>>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // Validating Key
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"])),

                        // Validating Issuer
                        ValidateIssuer = true,
                        ValidIssuer = _config["Token:Issuer"],

                        ValidateAudience = false
                    };
                });

            return services;
        }
    }
}
