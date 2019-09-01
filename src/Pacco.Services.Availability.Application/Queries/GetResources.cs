using System.Collections.Generic;
using Convey.CQRS.Queries;
using Pacco.Services.Availability.Application.DTO;

namespace Pacco.Services.Availability.Application.Queries
{
    public class GetResources : IQuery<IEnumerable<ResourceDto>>
    {
    }
}