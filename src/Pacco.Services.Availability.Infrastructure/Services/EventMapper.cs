using System.Collections.Generic;
using System.Linq;
using Convey.CQRS.Events;
using Pacco.Services.Availability.Application.Events;
using Pacco.Services.Availability.Application.Services;
using Pacco.Services.Availability.Core.Events;
using ReservationCanceled = Pacco.Services.Availability.Core.Events.ReservationCanceled;
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
                case ResourceCreated e: return new ResourceAdded(e.Resource.Id);
                case ResourceDeleted e: return new Application.Events.ResourceDeleted(e.Resource.Id);
                case ReservationAdded e: return new ResourceReserved(e.Resource.Id, e.Reservation.CustomerId, 
                    e.Reservation.OrderId, e.Reservation.DateTime);
                case ReservationReleased e: return new ResourceReleased(e.Resource.Id, e.Reservation.DateTime);
                case ReservationCanceled e: return new ResourceReservationCanceled(e.Resource.Id, 
                    e.Reservation.CustomerId, e.Reservation.DateTime);
            }
            return null;
        }
    }
}