using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductGuard.Database;
using ProductGuard.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductGuard.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly SimplyDbContext _context;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(SimplyDbContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Retrieves all products.")]
        [SwaggerResponse(200, "The products were retrieved successfully.")]
        [SwaggerResponse(404, "No products were found.")]
        [SwaggerResponse(500, "An error occurred while retrieving the products.")]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            try
            {
                var products = new List<ProductBase>();

                // Retrieve products from all types
                var cpuProducts = await _context.CPUs.ToListAsync();
                var gpuProducts = await _context.GPUs.ToListAsync();
                var motherboardProducts = await _context.Motherboards.ToListAsync();
                var ramProducts = await _context.RAMs.ToListAsync();
                var storageProducts = await _context.StorageDevices.ToListAsync();
                var psuProducts = await _context.PowerSupplies.ToListAsync();


                // Merge products from all types into a single list
                products.AddRange(cpuProducts);
                products.AddRange(gpuProducts);
                products.AddRange(motherboardProducts);
                products.AddRange(ramProducts);
                products.AddRange(storageProducts);
                products.AddRange(psuProducts);
                

                if (products.Any())
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

        // Other controller methods for specific product types...
    }
}
