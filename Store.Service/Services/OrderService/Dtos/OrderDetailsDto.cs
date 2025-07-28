using Store.Data.Entities.OrderEntities;

namespace Store.Service.Services.OrderService.Dtos
{
    public class OrderDetailsDto
    {
        public Guid Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public ShippingAddress ShippingAddress { get; set; }
        public string DeliveryMethodName { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Placed;
        public OrderPaymentStatus OrderPaymentStatus { get; set; } = OrderPaymentStatus.Pending;
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public decimal ShippingPrice { get; set; }
        public string? BasketId { get; set; }
        public string? PaymentIntentId { get; set; }
    }
}
