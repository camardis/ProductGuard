using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductGuard.Database;
using ProductGuard.Models;

namespace ProductGuard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GPUController : ProductBaseController<GPU>
    {
        public GPUController(SimplyDbContext context, ILogger<ProductBaseController<GPU>> logger) 
            : base(context, logger)
        {
        }
    }
}
