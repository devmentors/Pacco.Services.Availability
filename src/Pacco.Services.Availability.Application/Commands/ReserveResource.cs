using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Availability.Application.Commands
{
    public class ReserveResource : ICommand
    {
        public Guid ResourceId { get; }
        public int Priority { get; }
        public DateTime DateTime { get; }

        public ReserveResource(Guid resourceId, int priority, DateTime dateTime)
        {
            ResourceId = resourceId;
            Priority = priority;
            DateTime = dateTime;
        }
    }
}