using Microsoft.AspNetCore.Mvc;
using ProductGuard.Database;
using ProductGuard.Models;

namespace ProductGuard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PowerSuppliesController : ProductBaseController<PowerSupply>
    {
        public PowerSuppliesController(SimplyDbContext context, ILogger<ProductBaseController<PowerSupply>> logger) 
            : base(context, logger)
        {

        }
    }
}
