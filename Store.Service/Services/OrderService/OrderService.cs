using AutoMapper;
using Store.Data.Entities;
using Store.Data.Entities.OrderEntities;
using Store.Repository.Basket.Models;
using Store.Repository.Interfaces;
using Store.Repository.Specification.OrderSpecifications;
using Store.Service.Services.BasketService;
using Store.Service.Services.OrderService.Dtos;
using Store.Service.Services.PaymentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketService _basketService;
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork,
                            IBasketService basketService,
                            IPaymentService paymentService,
                            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _basketService = basketService;
            _paymentService = paymentService;
            _mapper = mapper;
        }
        public async Task<OrderDetailsDto> CreateOrderAsync(OrderDto input)
        {
            #region Get Baskket

            var basket = await _basketService.GetBasketAsync(input.BasketId);
            if (basket is null)
                throw new Exception($"the Basket does not exist");

            #endregion

            #region Handling OrderItems

            var orderItems = new List<OrderItemDto>();

            foreach(var basketItem in basket.BasketItems)
            {
                var product = await _unitOfWork.Repository<Product, int>().GetByIdAsync(basketItem.ProductId);
                if (product is null)
                    throw new Exception($"Product with ${basketItem.ProductId} does not exsit");

                var productItem = new ProductItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    PictureUrl = product.PictureUrl
                };

                var orderItem = new OrderItem
                {
                    Price = product.Price,
                    Quantity = basketItem.Quantity,
                    ProductItem = productItem
                };

                var mappedOrderItem = _mapper.Map<OrderItemDto>(orderItem);

                orderItems.Add(mappedOrderItem);
            }

            #endregion

            #region Get Delivery Method
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(input.DeliveryMethodId);

            if (deliveryMethod is null)
                throw new Exception("Delivery Method is not Provided");

            #endregion

            #region subtotal
            var subTotal = orderItems.Sum(item => item.Quantity * item.Price);
            #endregion

            #region Handling Payment

            var specs = new OrderWithPaymentIntentSpedification(basket.PaymentIntentId);
            var existingOrder = await _unitOfWork.Repository<Order, Guid>().GetWithSpecificationsByIdAsync(specs);
            if (existingOrder is null)
                await _paymentService.CreateOrUpdatePaymentIntent(basket);

            #endregion

            #region Creating Order

            var shippingAddress = _mapper.Map<ShippingAddress>(input.ShippingAddress);
            var mappedOrderItems = _mapper.Map<List<OrderItem>>(orderItems);

            var order = new Order
            {
                BasketId = input.BasketId,
                ShippingAddress = shippingAddress,
                DeliveryMethodid = input.DeliveryMethodId,
                OrderItems = mappedOrderItems,
                BuyerEmail = input.BuyerEmail,
                SubTotal = subTotal,
                PaymentIntentId = basket.PaymentIntentId
            };

            await _unitOfWork.Repository<Order, Guid>().AddAsync(order);
            await _unitOfWork.CompleteAsync();

            var mappedOrder = _mapper.Map<OrderDetailsDto>(order);
            return mappedOrder;
            #endregion
        }
        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
            => await _unitOfWork.Repository<DeliveryMethod, int>().GetAllAsync();

        public async Task<IReadOnlyList<OrderDetailsDto>> GetAllOrdersForUserAsync(string buyerEmail)
        {
            var specs = new OrderWithItemSpecification(buyerEmail);
            var userOrders = await _unitOfWork.Repository<Order, Guid>().GetAllWithSpecificationsAsync(specs);

            if (!userOrders.Any()) // or if(userOrders is {Count: <= 0})
                throw new Exception("You don't have any order yet!");

            var mappedOrders = _mapper.Map<IReadOnlyList<OrderDetailsDto>>(userOrders);
            return mappedOrders;
        }


        public async Task<OrderDetailsDto> GetOrderByIdAsync(Guid id)
        {
            var specs = new OrderWithItemSpecification(id);
            var order = await _unitOfWork.Repository<Order, Guid>().GetWithSpecificationsByIdAsync(specs);
            if (order is null)
                throw new Exception($"The Order with {id} does no exist!");
            var mappedOrder = _mapper.Map<OrderDetailsDto>(order);
            return mappedOrder;
        }
    }
}
