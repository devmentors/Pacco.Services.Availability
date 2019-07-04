using System;
using System.Collections.Generic;

namespace Pacco.Services.Availability.Application.DTO
{
    public class ResourceDto
    {
        public Guid Id { get; set; }
        public IEnumerable<ReservationDto> Reservation { get; set; }
    }

    public class ReservationDto
    {
        public DateTime DateTime { get; set; }
        public bool BelongsToVip { get; set; }
    }
}