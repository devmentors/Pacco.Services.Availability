using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Availability.Application.Events
{
    public class ResourceAdded : IEvent
    {
        public Guid Id { get; }

        public ResourceAdded(Guid id)
            => Id = id;
    }
}