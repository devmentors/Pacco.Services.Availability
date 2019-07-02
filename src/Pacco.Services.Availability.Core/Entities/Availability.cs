using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using Pacco.Services.Availability.Core.Events;
using Pacco.Services.Availability.Core.Exceptions;
using Pacco.Services.Availability.Core.ValueObjects;

namespace Pacco.Services.Availability.Core.Entities
{
    public class Availability : AggregateRoot
    {
        public Guid ResourceId { get; private set; }

        public IEnumerable<Reservation> Reservations
        {
            get => _reservations;
            private set => _reservations = new HashSet<Reservation>(value);
        }
        
        private ISet<Reservation> _reservations = new HashSet<Reservation>();

        public Availability(Guid id , Guid resourceId, IEnumerable<Reservation> reservations = null)
        {
            Id = id;
            ResourceId = resourceId;
            Reservations = reservations ?? Enumerable.Empty<Reservation>();
        }

        public static Availability Create(Guid id, Guid resourceId, IEnumerable<Reservation> reservations = null)
        {
            var availability = new Availability(id, resourceId, reservations);
            availability.AddEvent(new AvailabilityCreated(availability));
            return availability;
        }

        public void AddReservation(Reservation reservation, bool canBeExpropriated = false)
        {
            var hasCollidingReservation = _reservations.Any(HasTheSameReservationDate);

            if (hasCollidingReservation && canBeExpropriated)
            {
                var reservationToExpropriate = _reservations.First(HasTheSameReservationDate);

                if (reservationToExpropriate.BelongsToVip)
                {
                    throw new CannotExpropriateVipReservationException(reservationToExpropriate.CustomerId);
                }
                
                _reservations.Remove(reservationToExpropriate);
                _reservations.Add(reservation);
                AddEvent(new AvailabilityReservationExpropriated(this, reservationToExpropriate));
            }
            else if(hasCollidingReservation)
            {
                throw new CollidingReservationException(reservation.CustomerId);
            }

            if (!_reservations.Add(reservation))
            {
                return;
            }
            
            AddEvent(new AvailabilityReservationAdded(this, reservation));

            bool HasTheSameReservationDate(Reservation r)
                => r.DateTime.Date == reservation.DateTime.Date;
        }
    }
}