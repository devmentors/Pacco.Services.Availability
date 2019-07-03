using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Availability.Application.Events
{
    public class ReservationCanceled : IEvent
    {
        public Guid ResourceId { get; }
        public Guid CustomerId { get; }
        public DateTime DateTime { get; }

        public ReservationCanceled(Guid resourceId, Guid customerId, DateTime dateTime)
        {
            ResourceId = resourceId;
            CustomerId = customerId;
            DateTime = dateTime;
        }
    }
}