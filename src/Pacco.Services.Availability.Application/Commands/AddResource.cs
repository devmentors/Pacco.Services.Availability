using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Availability.Application.Commands
{
    [Contract]
    public class AddResource : ICommand
    {
        public Guid ResourceId { get; }

        public AddResource(Guid resourceId)
        {
            ResourceId = resourceId == Guid.Empty ? Guid.NewGuid() : resourceId;
        }
    }
}