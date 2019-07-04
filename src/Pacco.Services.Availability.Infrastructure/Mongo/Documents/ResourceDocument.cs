using System;
using System.Collections.Generic;
using Convey.Types;

namespace Pacco.Services.Availability.Infrastructure.Mongo.Documents
{
    internal class ResourceDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public IEnumerable<ReservationDocument> Reservations { get; set; }
    }

    internal class ReservationDocument
    {
        public DateTime DateTime { get; set; }
        public Guid CustomerId { get; set; }
        public Guid OrderId { get; set; }
        public bool BelongsToVip { get; set; }
    }
}