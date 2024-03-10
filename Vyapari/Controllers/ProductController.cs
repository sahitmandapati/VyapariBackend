using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Vyapari.Data.Entities;
using Vyapari.Infra;
using Vyapari.Service;

namespace Vyapari.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        private readonly IMapper _mapper;

        public ProductController(IMapper mapper, IProductService productService)
        {
            _mapper = mapper;
            _productService = productService;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            return Ok(await _productService.GetAllProducts());
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // [HttpPost]
        // public async Task<IActionResult> PostProduct(ProductRequest productRequest)
        // {
        //     var product = new Product
        //     {
        //         Name = productRequest.Name,
        //         Description = productRequest.Description,
        //         Price = productRequest.Price,
        //         QuantityAvailable = productRequest.QuantityAvailable,
        //         ImageUrl = productRequest.ImageUrl
        //     };

        //     await _productService.CreateProduct(product);

        //     return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        // }

        // POST: api/Product
        [HttpPost]
        public async Task<IActionResult> PostProduct(ProductRequest productRequest)
        {
            var product = _mapper.Map<Product>(productRequest);

            await _productService.CreateProduct(product);

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            await _productService.UpdateProduct(id, product);

            return NoContent();
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProduct(id);
            return NoContent();
        }
    }
}