using System;
using System.Threading.Tasks;
using Convey.CQRS.Events;

namespace Pacco.Services.Availability.Application.Events.External.Handlers
{
    public sealed class SignedUpHandler : IEventHandler<SignedUp>
    {
        public Task HandleAsync(SignedUp @event)
            => Task.CompletedTask;
    }
}