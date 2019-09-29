using System;
using System.Collections.Generic;
using System.Linq;
using Pacco.Services.Availability.Core.Events;
using Pacco.Services.Availability.Core.ValueObjects;

namespace Pacco.Services.Availability.Core.Entities
{
    public class Resource : AggregateRoot
    {
        public IEnumerable<Reservation> Reservations
        {
            get => _reservations;
            private set => _reservations = new HashSet<Reservation>(value);
        }
        
        private ISet<Reservation> _reservations = new HashSet<Reservation>();

        public Resource(Guid id , IEnumerable<Reservation> reservations = null, int? version = null)
        {
            Id = id;
            Reservations = reservations ?? Enumerable.Empty<Reservation>();
            Version = version ?? 0;
        }

        public static Resource Create(Guid id, IEnumerable<Reservation> reservations = null)
        {
            var resource = new Resource(id, reservations);
            resource.AddEvent(new ResourceCreated(resource));
            return resource;
        }

        public void AddReservation(Reservation reservation)
        {
        }

        public void ReleaseReservation(Reservation reservation)
        {
        }

        public void Delete()
        {
        }
    }
}