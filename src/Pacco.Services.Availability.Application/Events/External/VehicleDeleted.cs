using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Pacco.Services.Availability.Application.Events.External
{
    [MessageNamespace("vehicles")]
    public class VehicleDeleted : IEvent
    {
        public Guid Id { get; }

        public VehicleDeleted(Guid id)
            => Id = id;
    }
}