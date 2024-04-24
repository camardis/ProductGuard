using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductGuard.Database;
using ProductGuard.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace ProductGuard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageDeviceController : ControllerBase
    {
        private readonly SimplyDbContext _context;
        private readonly ILogger<StorageDeviceController> _logger;

        public StorageDeviceController(SimplyDbContext context, ILogger<StorageDeviceController> logger)
        {
            _context = context;
            _logger = logger;            
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Retrieves all storage device objects.")]
        [SwaggerResponse(200, "Returns a list of all storage device objects.", typeof(IEnumerable<StorageDevice>))]
        public async Task<IActionResult> GetStorageDevicesAsync()
        {
            try
            {
                _logger.LogInformation($"Attempting to retrieve all storage device objects. HTTP method: {HttpContext.Request.Method}, Path: {HttpContext.Request.Path}");
                var storageDeviceList = await _context.StorageDevices.ToListAsync();
                _logger.LogInformation($"Successfully retrieved all storage device objects. Response: {JsonConvert.SerializeObject(storageDeviceList)}");
                return Ok(storageDeviceList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving storage device objects.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{uuid}")]
        [SwaggerOperation(Summary = "Retrieves a storage device object by its UUID.")]
        [SwaggerResponse(200, "Returns the storage device object with the specified UUID.", typeof(StorageDevice))]
        [SwaggerResponse(404, "If the storage device object with the specified UUID does not exist.")]
        public async Task<IActionResult> GetStorageDeviceByUuidAsync([FromRoute] Guid uuid)
        {
            try
            {
                _logger.LogInformation($"Attempting to retrieve a storage device object by its UUID. HTTP method: {HttpContext.Request.Method}, Path: {HttpContext.Request.Path}, UUID: {uuid}");
                var storageDevice = await _context.StorageDevices.FindAsync(uuid);
                if (storageDevice == null)
                {
                    _logger.LogWarning($"Storage device object with UUID {uuid} not found. Returning 404 Not Found.");
                    return NotFound();
                }
                _logger.LogInformation($"Successfully retrieved storage device object by UUID. Response: {JsonConvert.SerializeObject(storageDevice)}");
                return Ok(storageDevice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving storage device object by UUID.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new storage device object.")]
        [SwaggerResponse(201, "Returns the created storage device object.", typeof(StorageDevice))]
        [SwaggerResponse(400, "If the request is invalid.")]
        public async Task<IActionResult> CreateStorageDeviceAsync([FromBody] StorageDevice storageDevice)
        {
            try
            {
                _logger.LogInformation($"Attempting to create a new storage device object. HTTP method: {HttpContext.Request.Method}, Path: {HttpContext.Request.Path}, Request Body: {JsonConvert.SerializeObject(storageDevice)}");
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid storage device object. Returning 400 Bad Request.");
                    return BadRequest("Invalid storage device object.");
                }
                await _context.StorageDevices.AddAsync(storageDevice);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Successfully created a new storage device object. Response: {JsonConvert.SerializeObject(storageDevice)}");
                return CreatedAtAction(nameof(GetStorageDevicesAsync), new { uuid = storageDevice.Uuid }, storageDevice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a storage device object.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{uuid}")]
        [SwaggerOperation(Summary = "Updates a storage device object.")]
        [SwaggerResponse(200, "Returns the updated storage device object.", typeof(StorageDevice))]
        [SwaggerResponse(400, "If the request is invalid.")]
        [SwaggerResponse(404, "If the storage device object does not exist.")]
        public async Task<IActionResult> UpdateStorageDeviceAsync([FromRoute] Guid uuid, [FromBody] StorageDevice storageDevice)
        {
            try
            {
                _logger.LogInformation($"Attempting to update a storage device object. HTTP method: {HttpContext.Request.Method}, Path: {HttpContext.Request.Path}, Request Body: {JsonConvert.SerializeObject(storageDevice)}");
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid storage device object. Returning 400 Bad Request.");
                    return BadRequest("Invalid storage device object.");
                }
                var existingStorageDevice = await _context.StorageDevices.FirstOrDefaultAsync(x => x.Uuid == uuid);
                if (existingStorageDevice == null)
                {
                    _logger.LogWarning("Storage device object not found. Returning 404 Not Found.");
                    return NotFound("Storage device object not found.");
                }
                existingStorageDevice.Name = storageDevice.Name;
                existingStorageDevice.Capacity = storageDevice.Capacity;
                existingStorageDevice.Type = storageDevice.Type;
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Successfully updated a storage device object. Response: {JsonConvert.SerializeObject(existingStorageDevice)}");
                return Ok(existingStorageDevice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a storage device object.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{uuid}")]
        [SwaggerOperation(Summary = "Deletes a storage device object.")]
        [SwaggerResponse(204, "If the storage device object was successfully deleted.")]
        [SwaggerResponse(404, "If the storage device object does not exist.")]
        public async Task<IActionResult> DeleteStorageDeviceAsync([FromRoute] Guid uuid)
        {
            try
            {
                _logger.LogInformation($"Attempting to delete a storage device object. HTTP method: {HttpContext.Request.Method}, Path: {HttpContext.Request.Path}");
                var storageDevice = await _context.StorageDevices.FirstOrDefaultAsync(x => x.Uuid == uuid);
                if (storageDevice == null)
                {
                    _logger.LogWarning("Storage device object not found. Returning 404 Not Found.");
                    return NotFound("Storage device object not found.");
                }
                _context.StorageDevices.Remove(storageDevice);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully deleted a storage device object.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a storage device object.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
