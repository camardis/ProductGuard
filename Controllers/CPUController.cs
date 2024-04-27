using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductGuard.Database;
using ProductGuard.Models;

namespace ProductGuard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CPUController : ProductBaseController<CPU>
    {
        public CPUController(SimplyDbContext context, ILogger<ProductBaseController<CPU>> logger) 
            : base(context, logger)
        {
        }
    }
}
