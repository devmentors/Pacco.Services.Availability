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
            var resource = await _queryDispatcher.QueryAsync(query);
            if (resource is null)
            {
                return NotFound();
            }

            return Ok(resource);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResourceDto>>> Get([FromQuery] GetResources query)
            => Ok(await _queryDispatcher.QueryAsync(query));

        [HttpPost]
        public async Task<ActionResult> Post(AddResource command)
        {
            await _commandDispatcher.SendAsync(command);
            return Created($"resources/{command.ResourceId}", null);
        }
    }
}