using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification.ProductSpecifications
{
    public class ProductWithSpecifications : BaseSpecification<Product>
    {
        public ProductWithSpecifications(ProductSpecification specs):
            base(product => ((!specs.BrandId.HasValue)||(product.BrandId == specs.BrandId.Value))&&
                            ((!specs.TypeId.HasValue) || (product.BrandId == specs.TypeId.Value)))
        {
            AddIncludes(p => p.Brand);
            AddIncludes(p => p.Type);

            AddOrderBy(p => p.Name);

            if (!string.IsNullOrEmpty(specs.Sort))
            {
                switch (specs.Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDesending(p => p.Price);
                        break;
                    case "Id":
                        AddOrderBy(p => p.Id);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }

            }
        }
        public ProductWithSpecifications(int? id) :
           base(product => product.Id == id)
        {
            AddIncludes(p => p.Brand);
            AddIncludes(p => p.Type);

        }
    }
}
