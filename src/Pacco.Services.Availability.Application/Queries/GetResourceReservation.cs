using System;
using Convey.CQRS.Queries;
using Pacco.Services.Availability.Application.DTO;

namespace Pacco.Services.Availability.Application.Queries
{
    public class GetResourceReservation : IQuery<ResourceDto>
    {
        public Guid Id { get; set; }
    }
}