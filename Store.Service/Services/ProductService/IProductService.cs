using Store.Repository.Specification.ProductSpecifications;
using Store.Service.Services.ProductService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.ProductService
{
    public interface IProductService
    {
        Task<ProductDetailsDto> GetProductByIdAsync(int? id);
        Task<IReadOnlyList<ProductDetailsDto>> GetAllProductsAsync(ProductSpecification specs);
        Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllBrandsAsync();
        Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllTypesAsync();

    }
}
