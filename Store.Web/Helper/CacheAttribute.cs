using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.Service.Services.CacheService;
using System.Text;

namespace Store.Web.Helper
{
    public class CacheAttribute  : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;

        public CacheAttribute(int timeToLiveInSeconds)
        {
            _timeToLiveInSeconds = timeToLiveInSeconds;
        }

        // before calling 
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 1- use CacheService
            var cacheSerivce = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            // 2- generate the cache key string
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            // 3- get response 
            var cacheResponse = await cacheSerivce.GetCacheResponseAsync(cacheKey);

            // 4- check if it was in memory (cached) -> Generate contentResult to be set for context.result
            if (!string.IsNullOrEmpty(cacheResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };

                context.Result = contentResult;

                return; // no need to continue
            }

            // if it was not found in the cache then go to the next action and get the data
            // and set it in cache --> for the first time case

            var executedContext = await next();

            if (executedContext.Result is OkObjectResult response)
            {
                await cacheSerivce.SetCacheResponseAsync(cacheKey, response.Value, TimeSpan.FromSeconds(_timeToLiveInSeconds));
            }



        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            StringBuilder cacheKey = new StringBuilder();
            cacheKey.Append($"{request.Path}");
            // request may contain parameters in a form of (key, value pair) -> we can use tuple
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                cacheKey.Append($"|{key}-{value}");
            }

            return cacheKey.ToString();
        }
    }
}
