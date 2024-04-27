using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductGuard.Database;
using ProductGuard.Models;

namespace ProductGuard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RAMController : ProductBaseController<RAM>
    {
        public RAMController(SimplyDbContext context, ILogger<ProductBaseController<RAM>> logger) 
            : base(context, logger)
        {

        }



    }
}
