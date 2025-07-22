using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.OpenApi.Models;

namespace Store.Web.Extensions
{
    public static class SwaggerServiceExtension
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "StoreAppApi",
                        Version = "v1",
                        Contact = new OpenApiContact
                        {
                            Name = "Hana Khaled",
                            Email = "hana.khaled.alsayed@gmail.com"
                        }
                    });

                // Handling security scheme (pop up)
                var securityScheme = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization Header using Bearer Scheme. Example:\" Authorization: Bearer {token} \"",
                    Name = "Authorization",
                    In = ParameterLocation.Header, // put token in request Header
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer",
                    // type , Id
                    Reference = new OpenApiReference
                    {
                        Id = "bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                };
                // Adding security scheme to options
                options.AddSecurityDefinition("bearer", securityScheme);


                // Handling Security Requirements (ex: must put "bearer" before token)
                var securityRequirements = new OpenApiSecurityRequirement
                {
                    {securityScheme, new []{"bearer"} }
                };
                // Adding security requirements to options
                options.AddSecurityRequirement(securityRequirements);
            });
            return services;
        }
    }
}
