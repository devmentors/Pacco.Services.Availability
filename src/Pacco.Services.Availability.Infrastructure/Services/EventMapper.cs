using System.Collections.Generic;
using System.Linq;
using Convey.CQRS.Events;
using Pacco.Services.Availability.Application.Events;
using Pacco.Services.Availability.Application.Services;
using Pacco.Services.Availability.Core.Events;
using ResourceDeleted = Pacco.Services.Availability.Core.Events.ResourceDeleted;

namespace Pacco.Services.Availability.Infrastructure.Services
{
    internal sealed class EventMapper : IEventMapper
    {
        public IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events)
            => events.Select(Map);
        
        public IEvent Map(IDomainEvent @event)
        {
            switch (@event)
            {
                case ResourceDeleted e: return new Application.Events.ResourceDeleted(e.Resource.Id);
            }
            return null;
        }
    }
}