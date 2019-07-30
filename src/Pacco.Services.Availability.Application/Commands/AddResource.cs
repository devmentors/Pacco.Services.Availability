using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Availability.Application.Commands
{
    public class AddResource : ICommand
    {
        public Guid ResourceId { get; }

        public AddResource(Guid resourceId)
            => ResourceId = resourceId;
    }
}