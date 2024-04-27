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
        [SwaggerOperation(Summary = "Retrieves all products of a type.")]
        [SwaggerResponse(200, "The products were retrieved successfully.")]
        [SwaggerResponse(404, "No products were found.")]
        [SwaggerResponse(500, "An error occurred while retrieving the products.")]
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
                    query = ascending ? query.OrderBy(x => EF.Property<object>(x, orderBy)) : query.OrderByDescending(x => EF.Property<object>(x, orderBy));
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
        [SwaggerOperation(Summary = "Retrieves a product by its uuid.")]
        [SwaggerResponse(200, "The product was retrieved successfully.")]
        [SwaggerResponse(404, "The product was not found.")]
        [SwaggerResponse(500, "An error occurred while retrieving the product.")]
        public async Task<IActionResult> GetProductByIdAsync([FromRoute] Guid uuid)
        {
            try
            {
                var product = await _context.Set<T>().FindAsync(uuid);
                if (product == null)
                {
                    _logger.LogWarning("Product with uuid {uuid} not found.", uuid);
                    return NotFound();
                }

                _logger.LogInformation("Product with uuid {uuid} found.", uuid);
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the product with uuid {uuid}.", uuid);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new product.")]
        [SwaggerResponse(201, "The product was created successfully.")]
        [SwaggerResponse(400, "The model state is invalid.")]
        [SwaggerResponse(500, "An error occurred while creating the product.")]
        public async Task<IActionResult> CreateProductAsync([FromBody] T product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state.");
                    return BadRequest(ModelState);
                }

                product.Id = await GenerateId();

                _context.Set<T>().Add(product);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Product created with uuid {uuid}.", product.Uuid);
                return Created("Product created", product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a product.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{uuid}")]
        [SwaggerOperation(Summary = "Updates a product by its uuid.")]
        [SwaggerResponse(204, "The product was updated successfully.")]
        [SwaggerResponse(400, "The model state is invalid.")]
        [SwaggerResponse(404, "The product was not found.")]
        [SwaggerResponse(500, "An error occurred while updating the product.")]
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
                    _logger.LogWarning("Product with uuid {uuid} not found.", uuid);
                    return NotFound();
                }

                _context.Entry(existingProduct).CurrentValues.SetValues(product);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Product with uuid {uuid} updated.", uuid);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the product with uuid {uuid}.", uuid);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{uuid}")]
        [SwaggerOperation(Summary = "Deletes a product by its uuid.")]
        [SwaggerResponse(204, "The product was deleted successfully.")]
        [SwaggerResponse(404, "The product was not found.")]
        [SwaggerResponse(500, "An error occurred while deleting the product.")]
        public async Task<IActionResult> DeleteProductAsync([FromRoute] Guid uuid)
        {
            try
            {
                var product = await _context.Set<T>().FindAsync(uuid);
                if (product == null)
                {
                    _logger.LogWarning("Product with uuid {uuid} not found.", uuid);
                    return NotFound();
                }

                _context.Set<T>().Remove(product);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Product with uuid {uuid} deleted.", uuid);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the product with uuid {uuid}.", uuid);
                return StatusCode(500, "Internal server error");
            }
        }

        // Set Stock amount of a product
        [HttpPut("{uuid}/stock")]
        [SwaggerOperation(Summary = "Updates the stock amount of a product by its uuid.")]
        [SwaggerResponse(204, "The stock amount was updated successfully.")]
        [SwaggerResponse(400, "The model state is invalid.")]
        [SwaggerResponse(404, "The product was not found.")]
        [SwaggerResponse(500, "An error occurred while updating the stock amount.")]
        public async Task<IActionResult> SetStockAsync([FromRoute] Guid uuid, [FromBody] int stockAmount)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state.");
                    return BadRequest(ModelState);
                }
                var product = await _context.Set<T>().FindAsync(uuid);
                if (product == null)
                {
                    _logger.LogWarning("Product with uuid {uuid} not found.", uuid);
                    return NotFound();
                }
                product.StockAmount = stockAmount;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Stock amount of product with uuid {uuid} updated.", uuid);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the stock amount of the product with uuid {uuid}.", uuid);
                return StatusCode(500, "Internal server error");
            }
        }

        // Increase or decrease the stock amount of a product
        [HttpPatch("{uuid}/stock")]
        [SwaggerOperation(Summary = "Increases or decreases the stock amount of a product by its uuid.")]
        [SwaggerResponse(204, "The stock amount was updated successfully.")]
        [SwaggerResponse(400, "The model state is invalid.")]
        [SwaggerResponse(404, "The product was not found.")]
        [SwaggerResponse(500, "An error occurred while updating the stock amount.")]
        public async Task<IActionResult> UpdateStockAsync([FromRoute] Guid uuid, [FromBody] int stockUpdate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state.");
                    return BadRequest(ModelState);
                }
                var product = await _context.Set<T>().FindAsync(uuid);
                if (product == null)
                {
                    _logger.LogWarning("Product with uuid {uuid} not found.", uuid);
                    return NotFound();
                }
                product.StockAmount += stockUpdate;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Stock amount of product with uuid {uuid} updated.", uuid);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the stock amount of the product with uuid {uuid}.", uuid);
                return StatusCode(500, "Internal server error");
            }
        }

        // Update the price of a product
        [HttpPut("{uuid}/price")]
        [SwaggerOperation(Summary = "Updates the price of a product by its uuid.")]
        [SwaggerResponse(204, "The price was updated successfully.")]
        [SwaggerResponse(400, "The model state is invalid.")]
        [SwaggerResponse(404, "The product was not found.")]
        [SwaggerResponse(500, "An error occurred while updating the price.")]
        public async Task<IActionResult> SetPriceAsync([FromRoute] Guid uuid, [FromBody] decimal price)
        {
            try
            {
                if (price <= 0)
                {
                    return BadRequest("Price must be greater than 0.");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state.");
                    return BadRequest(ModelState);
                }
                var product = await _context.Set<T>().FindAsync(uuid);
                if (product == null)
                {
                    _logger.LogWarning("Product with uuid {uuid} not found.", uuid);
                    return NotFound();
                }
                product.Price = price;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Price of product with uuid {uuid} updated.", uuid);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the price of the product with uuid {uuid}.", uuid);
                return StatusCode(500, "Internal server error");
            }
        }

        private async Task<int> GenerateId()
        {
            var highestId = await _context.Set<T>().MaxAsync(x => (int?)x.Id) ?? 0;
            return highestId + 1;
        }

    }
}
