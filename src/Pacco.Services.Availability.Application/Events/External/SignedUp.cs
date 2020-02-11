using System;
using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace Pacco.Services.Availability.Application.Events.External
{
    [Message(exchange: "identity")]
    public class SignedUp : IEvent
    {
        public Guid UserId { get; }
        public string Email { get; }
        
        public SignedUp(Guid userId, string email)
        {
            UserId = userId;
            Email = email;
        }
    }
}