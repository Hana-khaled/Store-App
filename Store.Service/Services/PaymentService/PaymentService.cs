using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Data.Entities;
using Store.Data.Entities.OrderEntities;
using Store.Repository.Interfaces;
using Store.Repository.Specification.OrderSpecifications;
using Store.Service.Services.BasketService;
using Store.Service.Services.BasketService.Dtos;
using Store.Service.Services.OrderService.Dtos;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Store.Data.Entities.Product;

namespace Store.Service.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketService _basketService;
        private readonly IMapper _mapper;

        public PaymentService( IConfiguration configuration,
                               IUnitOfWork unitOfWork,
                               IBasketService basketService,
                               IMapper mapper)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _basketService = basketService;
            _mapper = mapper;
        }
        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntent(CustomerBasketDto basket)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];

            // check if the basket is empty or not
            if (basket == null)
                throw new Exception("Basket is Empty");

            #region Handling Total Amount

            // total Amount: delivery method price + subtotal(price of item * its Quantity)

            #region delivery Method Price
            // delivery Method Price:
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodId.Value);

            if (deliveryMethod == null)
                throw new Exception("Delivery Method is not Provided");

            decimal shippingPrice = deliveryMethod.Price;

            #endregion

            #region Subtotal
            // subtotal:
            foreach (var item in basket.BasketItems)
            {
                var product = await _unitOfWork.Repository<Product, int>().GetByIdAsync(item.ProductId);
                // making sure that the price in redis memory matches the price in the database
                if (item.Price != product.Price)
                    item.Price = product.Price;
            }
            #endregion

            #endregion

            #region Handling Payment

            //Handling the payment service (new - update cases)
            //[Paymentservice takes options(amount, currency) -> has 2 types : create, Update
            //and returns paymentIntent(paymentIntentId, ClientSecret)] 

            // used for creating and Updating payment
            var service = new PaymentIntentService();
            // carry paymentIntent Id and ClientSecret
            PaymentIntent paymentIntent;

            //checking if it is a new payment or not
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                // new -> Create
                var options = new PaymentIntentCreateOptions
                {
                    // we (*100) because we want to convert from cent to dollar
                    Amount = (long)basket.BasketItems.Sum(item => (item.Price * 100) * item.Quantity) + (long)(shippingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }// card: Brandtype, 14 number, cvc, Date
                };

                // created for the first time
                paymentIntent = await service.CreateAsync(options);
                // update basket
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;

            }
            else
            {
                // already exists -> Update
                var options = new PaymentIntentUpdateOptions
                {
                    // here we need to check amout only if the payment already exists
                    Amount = (long)basket.BasketItems.Sum(item => (item.Price * 100) * item.Quantity) + (long)(shippingPrice * 100)
                };

                // Updating PaymentIntent
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }
            #endregion

            // update basket in database
            await _basketService.UpdateBasketAsync(basket);

            return basket;
        }

        public async Task<OrderDetailsDto> UpdateOrderPaymentFailed(string paymentIntentId)
        {
            var specs = new OrderWithPaymentIntentSpedification(paymentIntentId);

            var order = await _unitOfWork.Repository<Order, Guid>().GetWithSpecificationsByIdAsync(specs);

            if (order is null)
                throw new Exception("Order does not exist");

            order.OrderPaymentStatus = OrderPaymentStatus.Failed;

            _unitOfWork.Repository<Order, Guid>().Update(order);
            await _unitOfWork.CompleteAsync();

            var mappedOrder = _mapper.Map<OrderDetailsDto>(order);

            return mappedOrder;
        }

        public async Task<OrderDetailsDto> UpdateOrderPaymentSucceeded(string paymentIntentId)
        {
            var specs = new OrderWithPaymentIntentSpedification(paymentIntentId);

            var order = await _unitOfWork.Repository<Order, Guid>().GetWithSpecificationsByIdAsync(specs);

            if (order is null)
                throw new Exception("Order does not exist");

            order.OrderPaymentStatus = OrderPaymentStatus.Received;

            _unitOfWork.Repository<Order, Guid>().Update(order);
            await _unitOfWork.CompleteAsync();

            await _basketService.DeleteBasketAsync(order.BasketId);

            var mappedOrder = _mapper.Map<OrderDetailsDto>(order);

            return mappedOrder;
        }
    }
}
