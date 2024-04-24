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
using Newtonsoft.Json;

namespace ProductGuard.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RAMController : ControllerBase
    {
        private readonly SimplyDbContext _context;
        private readonly ILogger<RAMController> _logger;

        public RAMController(SimplyDbContext context, ILogger<RAMController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Retrieves all RAM objects.")]
        [SwaggerResponse(200, "Returns a list of all RAM objects.", typeof(IEnumerable<RAM>))]
        public async Task<IActionResult> GetRAMsAsync()
        {
            try
            {
                _logger.LogInformation($"Attempting to retrieve all RAM objects. HTTP method: {HttpContext.Request.Method}, Path: {HttpContext.Request.Path}");

                var ramList = await _context.RAMs.ToListAsync();

                _logger.LogInformation($"Successfully retrieved all RAM objects. Response: {JsonConvert.SerializeObject(ramList)}");

                return Ok(ramList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving RAM objects.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{uuid}")]
        [SwaggerOperation(Summary = "Retrieves a RAM object by its UUID.")]
        [SwaggerResponse(200, "Returns the RAM object with the specified UUID.", typeof(RAM))]
        [SwaggerResponse(404, "If the RAM object with the specified UUID does not exist.")]
        public async Task<IActionResult> GetRAMByUuidAsync([FromRoute] Guid uuid)
        {
            try
            {
                _logger.LogInformation($"Attempting to retrieve a RAM object by its UUID. HTTP method: {HttpContext.Request.Method}, Path: {HttpContext.Request.Path}, UUID: {uuid}");
                var ram = await _context.RAMs.FindAsync(uuid);
                if (ram == null)
                {
                    _logger.LogWarning($"RAM object with UUID {uuid} not found. Returning 404 Not Found.");
                    return NotFound();
                }
                _logger.LogInformation($"Successfully retrieved RAM object by UUID. Response: {JsonConvert.SerializeObject(ram)}");
                return Ok(ram);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving RAM object.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new RAM object.")]
        [SwaggerResponse(201, "Returns the created RAM object.", typeof(RAM))]
        [SwaggerResponse(400, "If the request is invalid.")]
        public async Task<IActionResult> CreateRAMAsync([FromBody] RAM ram)
        {
            try
            {
                _logger.LogInformation($"Attempting to create a new RAM object. HTTP method: {HttpContext.Request.Method}, Path: {HttpContext.Request.Path}, Request Body: {JsonConvert.SerializeObject(ram)}");

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state while creating RAM object.");
                    return BadRequest(ModelState);
                }

                _context.RAMs.Add(ram);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Successfully created a new RAM object. Response: {JsonConvert.SerializeObject(ram)}");

                return CreatedAtAction(nameof(GetRAMsAsync), new { uuid = ram.Uuid }, ram);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a RAM object.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{uuid}")]
        [SwaggerOperation(Summary = "Updates an existing RAM object.")]
        [SwaggerResponse(204, "If the RAM object is successfully updated.")]
        [SwaggerResponse(400, "If the request is invalid.")]
        [SwaggerResponse(404, "If the RAM object with the specified UUID is not found.")]
        public async Task<IActionResult> UpdateRAMByUuidAsync([FromRoute] Guid uuid, [FromBody] RAM ram)
        {
            try
            {
                _logger.LogInformation($"Attempting to update RAM object with UUID: {uuid}. HTTP method: {HttpContext.Request.Method}, Path: {HttpContext.Request.Path}, Request Body: {JsonConvert.SerializeObject(ram)}");

                if (uuid != ram.Uuid)
                {
                    _logger.LogWarning($"UUID in URL ({uuid}) does not match UUID in RAM object.");
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state while updating RAM object.");
                    return BadRequest(ModelState);
                }

                _context.Entry(ram).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Successfully updated RAM object with UUID: {uuid}.");

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.RAMs.Any(r => r.Uuid == uuid))
                {
                    _logger.LogWarning($"RAM object with UUID {uuid} not found.");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a RAM object.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{uuid}")]
        [SwaggerOperation(Summary = "Deletes a RAM object.")]
        [SwaggerResponse(204, "If the RAM object is successfully deleted.")]
        [SwaggerResponse(404, "If the RAM object with the specified UUID is not found.")]
        public async Task<IActionResult> DeleteRAMAsync([FromRoute] Guid uuid)
        {
            try
            {
                _logger.LogInformation($"Attempting to delete RAM object with UUID: {uuid}. HTTP method: {HttpContext.Request.Method}, Path: {HttpContext.Request.Path}");

                var ram = await _context.RAMs.FindAsync(uuid);
                if (ram == null)
                {
                    _logger.LogWarning($"RAM object with UUID {uuid} not found.");
                    return NotFound();
                }

                _context.RAMs.Remove(ram);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Successfully deleted RAM object with UUID: {uuid}.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a RAM object.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
