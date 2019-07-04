using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Availability.Application.Events
{
    public class ResourceReserved : IEvent
    {
        public Guid Id { get; }
        public DateTime DateTime { get; }

        public ResourceReserved(Guid id, DateTime dateTime)
        {
            Id = id;
            DateTime = dateTime;
        }
    }
}