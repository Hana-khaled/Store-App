using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Repository.Specification.ProductSpecifications;
using Store.Service.Services.ProductService;
using Store.Service.Services.ProductService.Dtos;
using Store.Web.Helper;

namespace Store.Web.Controllers
{
    [Route("api/[controller]/[action]")] //api/Products/GetAllBrands 
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // Ok() method comes from BaseController and works with ActionResult as a return type
        // it provides us with status code for the response
        // useful in caching

        [HttpGet]
        //[HttpGet("GetAllProducts")] // for specifiying route of api to avoid conflict between actions
        // but it is better to make it at [Route("api/[controller]")] instead of this(for each action)
        [Cache(10)]
        public async Task<ActionResult<IReadOnlyList<ProductDetailsDto>>> GetAllProductsAsync([FromQuery] ProductSpecification input)
            => Ok(await _productService.GetAllProductsAsync(input));

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<BrandTypeDetailsDto>>> GetAllBrandsAsync()
           => Ok(await _productService.GetAllBrandsAsync());

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<BrandTypeDetailsDto>>> GetAllTypesAsync()
           => Ok(await _productService.GetAllTypesAsync());

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDetailsDto>>> GetProductByIdAsync(int? id)
           => Ok(await _productService.GetProductByIdAsync(id));
    }
}
