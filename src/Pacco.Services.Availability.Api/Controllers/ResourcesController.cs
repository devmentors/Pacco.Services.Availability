using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Microsoft.AspNetCore.Mvc;
using Pacco.Services.Availability.Application.Commands;
using Pacco.Services.Availability.Application.DTO;
using Pacco.Services.Availability.Application.Queries;

namespace Pacco.Services.Availability.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourcesController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public ResourcesController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet("{resourceId}")]
        public async Task<ActionResult<ResourceDto>> Get([FromRoute] GetResource query)
        {
            var resource = await _queryDispatcher.QueryAsync<GetResource, ResourceDto>(query);

            if (resource is null)
            {
                return NotFound();
            }

            return Ok(resource);
        }
        
        [HttpGet]
        public async Task<ActionResult<ResourceDto>> Get([FromQuery] SearchResources query)
        {
            var resources = await _queryDispatcher.QueryAsync<SearchResources, IEnumerable<ResourceDto>>(query);

            if (resources is null)
            {
                return NotFound();
            }

            return Ok(resources);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AddResource command)
        {
            await _commandDispatcher.SendAsync(command);
            return Created($"resources/{command.ResourceId}", null);
        }

        [HttpPost("{resourceId}/reservations/{dateTime}")]
        public async Task<IActionResult> Post([FromBody] ReserveResource command)
        {
            await _commandDispatcher.SendAsync(command);
            return Ok();
        }
    }
}