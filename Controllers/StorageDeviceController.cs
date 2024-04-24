using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductGuard.Database;
using ProductGuard.Models;

namespace ProductGuard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageDeviceController : ProductBaseController<StorageDevice>
    {
        public StorageDeviceController(SimplyDbContext context, ILogger<ProductBaseController<StorageDevice>> logger) 
            : base(context, logger)
        {
        }


    }
}
