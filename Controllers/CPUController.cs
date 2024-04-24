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
    public class CPUController : ControllerBase
    {

        private readonly SimplyDbContext _context;
        private readonly ILogger<CPUController> _logger;

        public CPUController(SimplyDbContext context, ILogger<CPUController> logger)
        {
            _context = context;
            _logger = logger;            
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Retrieves all CPU objects.")]
        [SwaggerResponse(200, "Returns a list of all CPU objects.", typeof(IEnumerable<CPU>))]
        public async Task<IActionResult> GetCPUsAsync()
        {
            try
            {
                _logger.LogInformation($"Attempting to retrieve all CPU objects. HTTP method: {HttpContext.Request.Method}, Path: {HttpContext.Request.Path}");
                var cpuList = await _context.CPUs.ToListAsync();
                _logger.LogInformation($"Successfully retrieved all CPU objects. Response: {JsonConvert.SerializeObject(cpuList)}");
                return Ok(cpuList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving CPU objects.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{uuid}")]
        [SwaggerOperation(Summary = "Retrieves a CPU object by its UUID.")]
        [SwaggerResponse(200, "Returns the CPU object with the specified UUID.", typeof(CPU))]
        [SwaggerResponse(404, "If the CPU object with the specified UUID does not exist.")]
        public async Task<IActionResult> GetCPUByUuidAsync([FromRoute] Guid uuid)
        {
            try
            {
                _logger.LogInformation($"Attempting to retrieve a CPU object by its UUID. HTTP method: {HttpContext.Request.Method}, Path: {HttpContext.Request.Path}, UUID: {uuid}");
                var cpu = await _context.CPUs.FindAsync(uuid);
                if (cpu == null)
                {
                    _logger.LogWarning($"CPU object with UUID {uuid} not found. Returning 404 Not Found.");
                    return NotFound();
                }
                _logger.LogInformation($"Successfully retrieved CPU object by UUID. Response: {JsonConvert.SerializeObject(cpu)}");
                return Ok(cpu);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving CPU object.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new CPU object.")]
        [SwaggerResponse(201, "Returns the newly created CPU object.", typeof(CPU))]
        [SwaggerResponse(400, "If the request body is invalid.")]
        public async Task<IActionResult> CreateCPUAsync([FromBody] CPU cpu)
        {
            try
            {
                _logger.LogInformation($"Attempting to create a new CPU object. HTTP method: {HttpContext.Request.Method}, Path: {HttpContext.Request.Path}, Request body: {JsonConvert.SerializeObject(cpu)}");
                await _context.CPUs.AddAsync(cpu);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Successfully created new CPU object. Response: {JsonConvert.SerializeObject(cpu)}");
                return CreatedAtAction(nameof(GetCPUByUuidAsync), new { uuid = cpu.Uuid }, cpu);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating CPU object.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{uuid}")]
        [SwaggerOperation(Summary = "Updates a CPU object by its UUID.")]
        [SwaggerResponse(204, "If the CPU object was successfully updated.")]
        [SwaggerResponse(400, "If the request body is invalid.")]
        [SwaggerResponse(404, "If the CPU object with the specified UUID does not exist.")]
        public async Task<IActionResult> UpdateCPUByUuidAsync([FromRoute] Guid uuid, [FromBody] CPU cpu)
        {
            try
            {
                _logger.LogInformation($"Attempting to update a CPU object by its UUID. HTTP method: {HttpContext.Request.Method}, Path: {HttpContext.Request.Path}, UUID: {uuid}, Request body: {JsonConvert.SerializeObject(cpu)}");
                var existingCpu = await _context.CPUs.FindAsync(uuid);
                if (existingCpu == null)
                {
                    _logger.LogWarning($"CPU object with UUID {uuid} not found. Returning 404 Not Found.");
                    return NotFound();
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning($"Invalid request body. Returning 400 Bad Request. Errors: {JsonConvert.SerializeObject(ModelState.Values.SelectMany(v => v.Errors))}");
                    return BadRequest(ModelState);
                }

                _context.Entry(existingCpu).CurrentValues.SetValues(cpu);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Successfully updated CPU object by UUID. Response: {JsonConvert.SerializeObject(existingCpu)}");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating CPU object.");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
