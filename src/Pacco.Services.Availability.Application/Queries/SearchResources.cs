using System.Collections.Generic;
using Convey.CQRS.Queries;
using Pacco.Services.Availability.Application.DTO;

namespace Pacco.Services.Availability.Application.Queries
{
    public class SearchResources : IQuery<IEnumerable<ResourceDto>>
    {
        public IEnumerable<string> Tags { get; set; }
        public bool MatchAllTags { get; set; }
    }
}