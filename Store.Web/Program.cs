
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Data.Contexts;
using Store.Repository.Interfaces;
using Store.Repository.Repositories;
using Store.Service.HandleResponses;
using Store.Service.Services.ProductService;
using Store.Service.Services.ProductService.Dtos;
using Store.Web.Helper;
using Store.Web.Middleware;

namespace Store.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<StoreDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IProductService, ProductService>();

            builder.Services.AddAutoMapper(typeof(ProductProfile));

            builder.Services.Configure<ApiBehaviorOptions>(options =>
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

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            await ApplySeeding.ApplySeedingAsync(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
