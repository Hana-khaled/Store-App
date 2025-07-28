using Store.Data.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification.OrderSpecifications
{
    public class OrderWithPaymentIntentSpedification : BaseSpecification<Order>
    {
        public OrderWithPaymentIntentSpedification(string? paymentIntentId)
            : base(order => order.PaymentIntentId == paymentIntentId) 
        {
            
        }
    }
}
