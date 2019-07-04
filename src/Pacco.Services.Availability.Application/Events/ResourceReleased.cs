using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Availability.Application.Events
{
    public class ResourceReleased : IEvent
    {
        public Guid Id { get; }
        public DateTime DateTime { get; }

        public ResourceReleased(Guid id, DateTime dateTime)
        {
            Id = id;
            DateTime = dateTime;
        }
    }
}