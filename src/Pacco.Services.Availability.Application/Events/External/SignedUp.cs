using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Pacco.Services.Availability.Application.Events.External
{
    [Message(exchange: "identity")]
    public class SignedUp : IEvent
    {
        public string Email { get; }

        public SignedUp(string email)
        {
            Email = email;
        }
    }
}