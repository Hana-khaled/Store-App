using AutoMapper;
using Store.Repository.Basket;
using Store.Repository.Basket.Models;
using Store.Service.Services.BasketService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.BasketService
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
            => await _basketRepository.DeleteBasketAsync(basketId);

        public async Task<CustomerBasketDto> GetBasketAsync(string basketId)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);

            if (basket == null)
                return new CustomerBasketDto();

            return _mapper.Map<CustomerBasketDto>(basket);      
        }

        public async Task<CustomerBasketDto> UpdateBasketAsync(CustomerBasketDto basket)
        {
            if (basket.Id is null)
                basket.Id = GenerateBasketId();
            var mappedBasket = _mapper.Map<CustomerBasket>(basket);
            var updatedBasket = await _basketRepository.UpdateBasketAsync(mappedBasket);
            return _mapper.Map<CustomerBasketDto>(updatedBasket);

        }

        private string GenerateBasketId()
        {
            Random random = new Random();
            int randomDigits = random.Next(1000, 10000);
            return $"BS-{randomDigits}";
        }
    }
}
