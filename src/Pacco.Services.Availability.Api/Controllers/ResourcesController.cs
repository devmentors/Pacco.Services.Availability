using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Pacco.Services.Availability.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourcesController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Post()
        {
            await Task.CompletedTask;
            return Created($"resources/{Guid.NewGuid()}", null);
        }
    }
}