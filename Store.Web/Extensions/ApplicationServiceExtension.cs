using Microsoft.AspNetCore.Mvc;
using Store.Repository.Basket;
using Store.Repository.Interfaces;
using Store.Repository.Repositories;
using Store.Service.HandleResponses;
using Store.Service.Services.BasketService;
using Store.Service.Services.BasketService.Dtos;
using Store.Service.Services.CacheService;
using Store.Service.Services.OrderService;
using Store.Service.Services.OrderService.Dtos;
using Store.Service.Services.ProductService;
using Store.Service.Services.ProductService.Dtos;
using Store.Service.Services.TokenService;
using Store.Service.Services.UserService;

namespace Store.Web.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<IOrderService, OrderService>();

            services.AddScoped<ICacheService, CacheService>();

            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IBasketService, BasketService>();

            services.AddAutoMapper(typeof(ProductProfile));
            services.AddAutoMapper(typeof(BasketProfile));
            services.AddAutoMapper(typeof(OrderProfile));

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
