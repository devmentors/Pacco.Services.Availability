using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Pacco.Services.Availability.Application.Events;
using Pacco.Services.Availability.Application.Exceptions;
using Pacco.Services.Availability.Application.Services;
using Pacco.Services.Availability.Core.Entities;
using Pacco.Services.Availability.Core.Repositories;

namespace Pacco.Services.Availability.Application.Commands.Handlers
{
    public class AddResourceHandler : ICommandHandler<AddResource>
    {
        private readonly IResourcesRepository _resourcesRepository;
        private readonly IEventProcessor _eventProcessor;

        public AddResourceHandler(IResourcesRepository resourcesRepository, IEventProcessor eventProcessor)
        {
            _resourcesRepository = resourcesRepository;
            _eventProcessor = eventProcessor;
        }

        public async Task HandleAsync(AddResource command)
        {
            if (await _resourcesRepository.ExistsAsync(command.ResourceId))
            {
                throw new ResourceAlreadyExistsException(command.ResourceId);
            }

            var resource = Resource.Create(command.ResourceId, command.Tags);
            await _resourcesRepository.AddAsync(resource);
            await _eventProcessor.ProcessAsync(resource.Events);
        }
    }
}