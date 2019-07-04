using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Availability.Application.Events.Rejected
{
    public class ReserveResourceRejected : IRejectedEvent
    {
        public Guid Id { get; }
        public Guid CustomerId { get; }
        public Guid OrderId { get; }
        public DateTime DateTime { get; }
        public string Reason { get; }
        public string Code { get; }

        public ReserveResourceRejected(Guid id, Guid customerId, Guid orderId, DateTime dateTime, 
            string reason, string code)
        {
            Id = id;
            CustomerId = customerId;
            OrderId = orderId;
            DateTime = dateTime;
            Reason = reason;
            Code = code;
        }
    }
}