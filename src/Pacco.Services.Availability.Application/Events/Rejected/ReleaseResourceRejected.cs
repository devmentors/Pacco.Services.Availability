using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Availability.Application.Events.Rejected
{
    public class ReleaseResourceRejected : IRejectedEvent
    {
        public Guid Id { get; }
        public DateTime DateTime { get; }
        public string Reason { get; }
        public string Code { get; }

        public ReleaseResourceRejected(Guid id, DateTime dateTime, string reason, string code)
        {
            Id = id;
            DateTime = dateTime;
            Reason = reason;
            Code = code;
        }
    }
}