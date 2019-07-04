using System.Linq;
using Pacco.Services.Availability.Application.DTO;
using Pacco.Services.Availability.Core.Entities;
using Pacco.Services.Availability.Core.ValueObjects;

namespace Pacco.Services.Availability.Infrastructure.Mongo.Documents
{
    internal static class Extensions
    {
        public static Resource AsEntity(this ResourceDocument document)
            => new Resource(document.Id, document.Reservations
                .Select(r => new Reservation(r.DateTime, r.CustomerId, r.OrderId, r.BelongsToVip)), document.Version);
        
        public static ResourceDocument AsDocument(this Resource entity)
            => new ResourceDocument
            {
                Id = entity.Id,
                Version = entity.Version,
                Reservations = entity.Reservations.Select(r => new ReservationDocument
                {
                    DateTime = r.DateTime,
                    CustomerId = r.CustomerId,
                    OrderId = r.CustomerId,
                    BelongsToVip = r.BelongsToVip
                })
            };
        
        public static ResourceDto AsDto(this ResourceDocument document)
            => new ResourceDto
            {
                Id = document.Id,
                Reservations = document.Reservations.Select(r => new ReservationDto
                {
                    DateTime = r.DateTime,
                    BelongsToVip = r.BelongsToVip
                })
            };
    }
}