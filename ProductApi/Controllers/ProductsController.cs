using Microsoft.AspNetCore.Mvc;
using ProductApi.DTOs;
using ProductApi.Interfaces;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
      
            private readonly IProductService _service;

            public ProductsController(IProductService service) => _service = service;

            [HttpGet]
            public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

            [HttpGet("{id}")]
            public async Task<IActionResult> Get(string id)
            {
                var product = await _service.GetByIdAsync(id);
                return product == null ? NotFound() : Ok(product);
            }

            [HttpPost]
            public async Task<IActionResult> Create([FromBody] ProductDto dto)
            {
                var product = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = product.ProductId }, product);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> Update(string id, [FromBody] ProductDto dto)
            {
                var product = await _service.UpdateAsync(id, dto);
                return product == null ? NotFound() : Ok(product);
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(string id)
            {
                var success = await _service.DeleteAsync(id);
                return success ? Ok() : NotFound();
            }

            [HttpPut("decrement-stock/{id}/{quantity}")]
            public async Task<IActionResult> DecrementStock(string id, int quantity)
            {
                var success = await _service.DecrementStockAsync(id, quantity);
                return success ? Ok() : BadRequest();
            }

            [HttpPut("add-to-stock/{id}/{quantity}")]
            public async Task<IActionResult> AddToStock(string id, int quantity)
            {
                var success = await _service.AddStockAsync(id, quantity);
                return success ? Ok() : BadRequest();
            }
    }

}

