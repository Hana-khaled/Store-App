﻿using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification.ProductSpecifications
{
    public class ProductsCountSpecifications : BaseSpecification<Product>
    {
        public ProductsCountSpecifications(ProductSpecification specs) :
            base(product => ((!specs.BrandId.HasValue) || (product.BrandId == specs.BrandId.Value)) &&
                            ((!specs.TypeId.HasValue) || (product.BrandId == specs.TypeId.Value)) &&
                            ((string.IsNullOrEmpty(specs.Search)) || (product.Name.Trim().ToLower().Contains(specs.Search))))
        {
        }

    }
}
