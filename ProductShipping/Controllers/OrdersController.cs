using Microsoft.AspNetCore.Mvc;
using ProductShipping.Models;
using ProductShipping.Services;

namespace ProductShipping.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly PackageService _packageService;
        private readonly DBContext _dbcontext;

        public OrdersController(PackageService packageService, DBContext dbcontext)
        {
            _packageService = packageService;
            _dbcontext = dbcontext;
        }

        [HttpGet("products")]
        public IActionResult GetProducts()
        {
            var products = _dbcontext.Products.ToList();
            return Ok(products);
        }

        [HttpPost("placeorder")]
        public IActionResult PlaceOrder(List<Product> products)
        {
            if (products == null || !products.Any())
            {
                return BadRequest("No products provided.");
            }
            try
            {
                var result = _packageService.SplitPackages(products);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
