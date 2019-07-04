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
        public int TimeStamp { get; set; }
        public int Priority { get; set; }
    }
}