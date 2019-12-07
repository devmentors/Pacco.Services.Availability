using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Availability.Application.Commands
{
    [Contract]
    public class ReserveResource : ICommand
    {
        public Guid ResourceId { get; }
        public Guid CustomerId { get; }
        public DateTime DateTime { get; }
        public int Priority { get; }

        public ReserveResource(Guid resourceId, Guid customerId, DateTime dateTime, int priority)
        {
            ResourceId = resourceId;
            CustomerId = customerId;
            DateTime = dateTime;
            Priority = priority;
        }
    }
}