using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Store.Web.Controllers
{
    [Route("api/[controller]/[action]")] //api/Products/GetAllBrands 
    [ApiController]
    public class BaseController : ControllerBase
    {
    }
}
