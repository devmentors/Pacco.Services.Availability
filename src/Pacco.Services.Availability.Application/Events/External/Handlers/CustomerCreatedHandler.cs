using System;
using System.Threading.Tasks;
using Convey.CQRS.Events;

namespace Pacco.Services.Availability.Application.Events.External.Handlers
{
    public sealed class CustomerCreatedHandler : IEventHandler<CustomerCreated>
    {
        public Task HandleAsync(CustomerCreated @event)
            => Task.CompletedTask;
    }
}