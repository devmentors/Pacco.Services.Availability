using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Availability.Application.Events
{
    [Contract]
    public class ResourceReleased : IEvent
    {
        public Guid ResourceId { get; }
        public DateTime DateTime { get; }

        public ResourceReleased(Guid resourceId, DateTime dateTime)
            => (ResourceId, DateTime) = (resourceId, dateTime);
    }
}