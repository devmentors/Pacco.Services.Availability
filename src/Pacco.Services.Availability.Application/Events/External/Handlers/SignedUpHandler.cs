using System;
using System.Threading.Tasks;
using Convey.CQRS.Events;

namespace Pacco.Services.Availability.Application.Events.External.Handlers
{
    public sealed class SignedUpHandler : IEventHandler<SignedUp>
    {
        public async Task HandleAsync(SignedUp @event)
        {
            Console.WriteLine($"Received signed-up with email: {@event.Email}");
        }
    }
}