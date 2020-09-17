using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Availability.Application.Commands
{
    public class ReserveResource : ICommand
    {
        public Guid ResourceId { get; }
        public Guid CustomerId { get; }
        public int Priority { get; }
        public DateTime DateTime { get; }

        public ReserveResource(Guid resourceId, Guid customerId, int priority, DateTime dateTime)
        {
            ResourceId = resourceId;
            CustomerId = customerId;
            Priority = priority;
            DateTime = dateTime;
        }
    }
}