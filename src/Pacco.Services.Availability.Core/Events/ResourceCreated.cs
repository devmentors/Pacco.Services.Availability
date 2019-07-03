using Pacco.Services.Availability.Core.Entities;

namespace Pacco.Services.Availability.Core.Events
{
    public class ResourceCreated : IDomainEvent
    {
        public Resource Resource { get; }

        public ResourceCreated(Resource resource)
            => Resource = resource;
    }
}