using AutoMapper;
using Store.Data.Entities;
using Store.Repository.Interfaces;
using Store.Repository.Specification.ProductSpecifications;
using Store.Service.Services.ProductService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.Repository<ProductBrand, int>().GetAllAsNoTrackingAsync();

            var mappedBrands = _mapper.Map<IReadOnlyList<BrandTypeDetailsDto>>(brands);

            return mappedBrands;
        }

        public async Task<IReadOnlyList<ProductDetailsDto>> GetAllProductsAsync(ProductSpecification input)
        {
            var specs = new ProductWithSpecifications(input);
            var products = await _unitOfWork.Repository<Product, int>().GetAllWithSpecificationsAsync(specs);

            //var mappedProducts = products.Select(x => new ProductDetailsDto()
            //{
            //    Id = x.Id,
            //    Name = x.Name,
            //    Description = x.Description,
            //    Price = x.Price,
            //    PictureUrl = x.PictureUrl,
            //    BrandName = x.Brand.Name,
            //    TypeName = x.Type.Name,
            //    CreatedAt = x.CreatedAt

            //}).ToList();

            var mappedProducts = _mapper.Map<IReadOnlyList<ProductDetailsDto>>(products);

            return mappedProducts;
        }

        public async Task<IReadOnlyList<BrandTypeDetailsDto>> GetAllTypesAsync()
        {
            var types = await _unitOfWork.Repository<ProductType, int>().GetAllAsNoTrackingAsync();
            var mappedtypes = _mapper.Map<IReadOnlyList<BrandTypeDetailsDto>>(types);

            return mappedtypes;
        }

        public async Task<ProductDetailsDto> GetProductByIdAsync(int? id)
        {
            if (id is null)
                throw new Exception("id is null");

            var specs = new ProductWithSpecifications(id);

            var product = await _unitOfWork.Repository<Product, int>().GetWithSpecificationsByIdAsync(specs);

            if (product is null)
                throw new Exception("Product Not Found");

            var mappedProduct = _mapper.Map<ProductDetailsDto>(product);

            return mappedProduct;
        }
    }
}
