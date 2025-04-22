using Microsoft.AspNetCore.Mvc;
using Store.Repository.Interfaces;
using Store.Repository.Repositories;
using Store.Service.HandleResponses;
using Store.Service.Services.ProductService;
using Store.Service.Services.ProductService.Dtos;

namespace Store.Web.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductService, ProductService>();

            services.AddAutoMapper(typeof(ProductProfile));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                // at invalid model state we want to list errors
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                                 .Where(model => model.Value?.Errors.Count() > 0)
                                 // each property in model(name, age,...) may have its own list of errors 
                                 // list of list
                                 .SelectMany(model => model.Value?.Errors)
                                 .Select(error => error.ErrorMessage)
                                 .ToList();

                    // use the errors in custom validationErrorResponse
                    var errorResponse = new ValidationErrorResponse
                    {
                        Errors = errors
                    };

                    // send proper response to bad request 
                    return new BadRequestObjectResult(errorResponse);

                };
            });

            return services;
        }
    }
}
