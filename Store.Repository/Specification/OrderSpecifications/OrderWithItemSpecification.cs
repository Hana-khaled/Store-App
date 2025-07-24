using Store.Data.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification.OrderSpecifications
{
    public class OrderWithItemSpecification : BaseSpecification<Order>
    {
        public OrderWithItemSpecification(int buyerId) : base(order => order.BuyerId == buyerId)
        {
            AddIncludes(order => order.DeliveryMethod);
            AddIncludes(order => order.OrderItems);
            AddOrderByDesending(order => order.OrderDate);
        }

        public OrderWithItemSpecification(Guid orderId) : base(order => order.Id == orderId)
        {
            AddIncludes(order => order.DeliveryMethod);
            AddIncludes(order => order.OrderItems);
        }
    }
}
