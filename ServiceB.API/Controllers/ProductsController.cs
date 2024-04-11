using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServiceB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        [HttpGet("{id}")]
        public IActionResult GetProduct (int id)
        {
            return Ok(new { Id = id, Name = "Kalem", Price = 100, Stock = 200, Category = "Kalemler" });
        }
    }
}
