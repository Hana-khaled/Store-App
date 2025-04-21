using Store.Service.HandleResponses;
using System.Net;
using System.Text.Json;

namespace Store.Web.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _enviroment;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            IHostEnvironment enviroment,
            ILogger<ExceptionMiddleware> logger
            )
        {
            _next = next; // next middleware
            _enviroment = enviroment; // enviroment type (Development(need more details), Production)
            _logger = logger; // for logging error
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // try to move on to the next middleware
                await _next(context);
            }
            catch (Exception ex)
            {
                // 1- log the error
                _logger.LogError(ex, ex.Message);

                // 2- Handle response
                // -> through context(ContentType, StatusCode, WriteAsync(CustomException))
                // -> custom exception(based on enviroment)

                context.Response.ContentType = "application/json";

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                // Enum casted to int (500)

                // Handling Custom Exception
                var response = _enviroment.IsDevelopment() ?
                    new CustomException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace) :
                    new CustomException((int)HttpStatusCode.InternalServerError);

                // Optional step (json Naming policy)
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                // converting our response object into json format through serilization
                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);


            }
        }
    }
}
