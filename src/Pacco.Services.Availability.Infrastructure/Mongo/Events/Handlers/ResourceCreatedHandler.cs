using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Pacco.Services.Availability.Application.DTO;
using Pacco.Services.Availability.Application.Events;
using Pacco.Services.Availability.Core.Events;

namespace Pacco.Services.Availability.Infrastructure.Mongo.Events.Handlers
{
    public class ResourceCreatedHandler : IDomainEventHandler<ResourceCreated>
    {
        private readonly IMongoDatabase _database;

        public ResourceCreatedHandler(IMongoDatabase database)
        {
            _database = database;
        }
        
        public async Task HandleAsync(ResourceCreated @event)
        {
            var resourceDto = new ResourceDto
            {
                Id = @event.Resource.Id,
                Reservations = @event.Resource.Reservations.Select(r => new ReservationDto
                {
                    Priority = r.Priority,
                    DateTime = r.DateTime
                })
            };

            await _database.GetCollection<ResourceDto>("ResourcesDto")
                .InsertOneAsync(resourceDto);
        }
    }
}