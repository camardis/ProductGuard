using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductGuard.Database;
using ProductGuard.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
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
        public async Task<IActionResult> GetProductsAsync(int page = 1, int pageSize = 10, string orderBy = "Id", bool ascending = true)
        {
            try
            {
                var query = _context.Set<T>().AsQueryable();

                // Pagination
                query = query.Skip((page - 1) * pageSize).Take(pageSize);

                // Sorting
                var property = typeof(T).GetProperty(orderBy);
                if (property != null)
                {
                    query = ascending ? query.OrderBy(x => property.GetValue(x, null)) : query.OrderByDescending(x => property.GetValue(x, null));
                }

                var products = await query.ToListAsync();
                if (products != null && products.Any())
                {
                    _logger.LogInformation("Retrieved {Count} products.", products.Count);
                    return Ok(products);
                }

                _logger.LogWarning("No products were found.");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the products.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{uuid}")]
        [SwaggerOperation(Summary = "Retrieves a product by its ID.")]
        public async Task<IActionResult> GetProductByIdAsync([FromRoute] Guid uuid)
        {
            try
            {
                var product = await _context.Set<T>().FindAsync(uuid);
                if (product == null)
                {
                    _logger.LogWarning("Product with ID {Id} not found.", uuid);
                    return NotFound();
                }

                _logger.LogInformation("Product with ID {Id} found.", uuid);
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the product with ID {Id}.", uuid);
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
                    _logger.LogWarning("Invalid model state.");
                    return BadRequest(ModelState);
                }

                _context.Set<T>().Add(product);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Product created with ID {Id}.", product.Uuid);
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
        public async Task<IActionResult> UpdateProductAsync([FromRoute] Guid uuid, [FromBody] T product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state.");
                    return BadRequest(ModelState);
                }

                var existingProduct = await _context.Set<T>().FindAsync(uuid);
                if (existingProduct == null)
                {
                    _logger.LogWarning("Product with ID {Id} not found.", uuid);
                    return NotFound();
                }

                _context.Entry(existingProduct).CurrentValues.SetValues(product);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Product with ID {Id} updated.", uuid);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the product with ID {Id}.", uuid);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{uuid}")]
        [SwaggerOperation(Summary = "Deletes a product by its ID.")]
        public async Task<IActionResult> DeleteProductAsync([FromRoute] Guid uuid)
        {
            try
            {
                var product = await _context.Set<T>().FindAsync(uuid);
                if (product == null)
                {
                    _logger.LogWarning("Product with ID {Id} not found.", uuid);
                    return NotFound();
                }

                _context.Set<T>().Remove(product);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Product with ID {Id} deleted.", uuid);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the product with ID {Id}.", uuid);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
