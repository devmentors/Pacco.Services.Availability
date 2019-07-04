using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Availability.Application.Events
{
    public class ResourceReservationCanceled : IEvent
    {
        public Guid ResourceId { get; }
        public Guid CustomerId { get; }
        public DateTime DateTime { get; }

        public ResourceReservationCanceled(Guid resourceId, Guid customerId, DateTime dateTime)
        {
            ResourceId = resourceId;
            CustomerId = customerId;
            DateTime = dateTime;
        }
    }
}