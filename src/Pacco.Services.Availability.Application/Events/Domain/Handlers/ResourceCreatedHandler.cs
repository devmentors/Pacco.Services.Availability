using System.Threading.Tasks;
using Pacco.Services.Availability.Core.Events;

namespace Pacco.Services.Availability.Application.Events.Domain.Handlers
{
    public class ResourceCreatedHandler : IDomainEventHandler<ResourceCreated>
    {
        public async Task HandleAsync(ResourceCreated @event)
        {
            await Task.CompletedTask;
        }
    }
}