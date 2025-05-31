using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Service.Services.BasketService;
using Store.Service.Services.BasketService.Dtos;

namespace Store.Web.Controllers
{
    public class BasketController : BaseController
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasketDto>> GetBasketAsync(string basketId)
            => Ok(await _basketService.GetBasketAsync(basketId));

        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> UpdateBasketAsync(CustomerBasketDto basket)
            => Ok(await _basketService.UpdateBasketAsync(basket));

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasketAsync(string basketId)
            => Ok(await _basketService.DeleteBasketAsync(basketId));
    }
}
