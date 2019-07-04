using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Availability.Application.Events
{
    public class ResourceReserved : IEvent
    {
        public Guid Id { get; }
        public Guid CustomerId { get; }
        public Guid OrderId { get; }
        public DateTime DateTime { get; }

        public ResourceReserved(Guid id, Guid customerId, Guid orderId, DateTime dateTime)
        {
            Id = id;
            CustomerId = customerId;
            OrderId = orderId;
            DateTime = dateTime;
        }
    }
}