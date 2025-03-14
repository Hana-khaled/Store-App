using Store.Data.Entities;
using Store.Repository.Interfaces;
using Store.Service.Services.Product.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.Repository<ProductBrand, int>().GetAllAsNoTrackingAsync();

            var mappedBrands = brands.Select(x => new BrandTypeDetailsDto()
            {
                Id = x.Id,
                Name = x.Name,
                CreatedAt = x.CreatedAt
            }).ToList();

            return mappedBrands;
        }

        public async Task<IReadOnlyList<ProductDetailsDto>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.Repository<Store.Data.Entities.Product, int>().GetAllAsNoTrackingAsync();

            var mappedProducts = products.Select(x => new ProductDetailsDto()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                PictureUrl = x.PictureUrl,
                BrandName = x.Brand.Name,
                TypeName = x.Type.Name,
                CreatedAt = x.CreatedAt
                
            }).ToList();

            return mappedProducts;
        }

        public async Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllTypesAsync()
        {
            var types = await _unitOfWork.Repository<ProductType, int>().GetAllAsNoTrackingAsync();
            var mappedtypes = types.Select(x => new BrandTypeDetailsDto()
            {
                Id = x.Id,
                Name = x.Name,
                CreatedAt = x.CreatedAt
            }).ToList();

            return mappedtypes;
        }

        public async Task<ProductDetailsDto> GetProductByIdAsync(int? id)
        {
            if (id is null)
                throw new Exception("id is null");

            var product = await _unitOfWork.Repository<Store.Data.Entities.Product, int>().GetByIdAsync(id.Value);

            if (product is null)
                throw new Exception("Product Not Found");

            var mappedProduct = new ProductDetailsDto()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                PictureUrl = product.PictureUrl,
                BrandName = product.Brand.Name,
                TypeName = product.Type.Name,
                CreatedAt = product.CreatedAt
            };

            return mappedProduct;
        }
    }
}
