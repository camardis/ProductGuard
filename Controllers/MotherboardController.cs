using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductGuard.Database;
using ProductGuard.Models;

namespace ProductGuard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotherboardController : ProductBaseController<Motherboard>
    {
        public MotherboardController(SimplyDbContext context, ILogger<ProductBaseController<Motherboard>> logger) 
            : base(context, logger)
        {

        }
    }
}
