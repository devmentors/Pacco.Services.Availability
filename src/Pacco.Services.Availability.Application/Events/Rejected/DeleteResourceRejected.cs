using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Availability.Application.Events.Rejected
{
    public class DeleteResourceRejected : IRejectedEvent
    {
        public Guid Id { get; }
        public string Reason { get; }
        public string Code { get; }

        public DeleteResourceRejected(Guid id, string reason, string code)
        {
            Id = id;
            Reason = reason;
            Code = code;
        }
    }
}