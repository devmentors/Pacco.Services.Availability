using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Availability.Application.Events
{
    public class ResourceDeleted : IEvent
    {
        public Guid Id { get; }

        public ResourceDeleted(Guid id)
            => Id = id;
    }
}