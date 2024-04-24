using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductGuard.Database;
using ProductGuard.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace ProductGuard.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ProductBaseController<T> : ControllerBase where T : ProductBase
    {
        private readonly SimplyDbContext _context;
        private readonly ILogger<ProductBaseController<T>> _logger;

        public ProductBaseController(SimplyDbContext context, ILogger<ProductBaseController<T>> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Retrieves all products.")]
        public async Task<IActionResult> GetProductsAsync()
        {
            try
            {
                var products = await _context.Set<T>().ToListAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the products.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{uuid}")]
        [SwaggerOperation(Summary = "Retrieves a product by its ID.")]
        public async Task<IActionResult> GetProductByIdAsync([FromRoute] Guid Uuid)
        {
            try
            {
                var product = await _context.Set<T>().FindAsync(Uuid);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the product.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new product.")]
        [SwaggerResponse(201, "The product was created successfully.")]
        public async Task<IActionResult> CreateProductAsync([FromBody] T product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _context.Set<T>().Add(product);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProductByIdAsync), new { uuid = product.Uuid }, product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a product.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{uuid}")]
        [SwaggerOperation(Summary = "Updates a product by its ID.")]
        public async Task<IActionResult> UpdateProductAsync([FromRoute] Guid Uuid, [FromBody] T product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingProduct = await _context.Set<T>().FindAsync(Uuid);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                _context.Entry(existingProduct).CurrentValues.SetValues(product);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the product.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{uuid}")]
        [SwaggerOperation(Summary = "Deletes a product by its ID.")]
        public async Task<IActionResult> DeleteProductAsync([FromRoute] Guid Uuid)
        {
            try
            {
                var product = await _context.Set<T>().FindAsync(Uuid);
                if (product == null)
                {
                    return NotFound();
                }

                _context.Set<T>().Remove(product);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the product.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
