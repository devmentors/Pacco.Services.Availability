using System;
using System.Collections.Generic;
using System.Linq;
using Pacco.Services.Availability.Core.Events;
using Pacco.Services.Availability.Core.Exceptions;
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
            var availability = new Resource(id, reservations);
            availability.AddEvent(new ResourceCreated(availability));
            return availability;
        }

        public void AddReservation(Reservation reservation, bool canExpropriate = false)
        {
            var hasCollidingReservation = _reservations.Any(HasTheSameReservationDate);

            if (hasCollidingReservation && canExpropriate)
            {
                var reservationToExpropriate = _reservations.First(HasTheSameReservationDate);

                if (reservationToExpropriate.BelongsToVip)
                {
                    throw new CannotExpropriateVipReservationException(reservationToExpropriate.CustomerId);
                }
                
                _reservations.Remove(reservationToExpropriate);
                _reservations.Add(reservation);
                AddEvent(new ReservationCanceled(this, reservationToExpropriate));
            }
            else if(hasCollidingReservation)
            {
                throw new CollidingReservationException(reservation.CustomerId);
            }

            if (!_reservations.Add(reservation))
            {
                return;
            }
            
            AddEvent(new ReservationAdded(this, reservation));

            bool HasTheSameReservationDate(Reservation r)
                => r.DateTime.Date == reservation.DateTime.Date;
        }

        public void ReleaseReservation(Reservation reservation)
        {
            if (!_reservations.Remove(reservation))
            {
                return;
            }
            
            AddEvent(new ReservationReleased(this, reservation));
        }

        public void Delete()
        {
            foreach (var reservation in Reservations)
            {
                AddEvent(new ReservationCanceled(this, reservation));
            }
            
            AddEvent(new ResourceDeleted(this));
        }
    }
}