using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Pacco.Services.Availability.Application.Exceptions;
using Pacco.Services.Availability.Application.Services;
using Pacco.Services.Availability.Core.Entities;
using Pacco.Services.Availability.Core.Repositories;

namespace Pacco.Services.Availability.Application.Commands.Handlers
{
    public sealed class AddResourceHandler : ICommandHandler<AddResource>
    {
        private readonly IResourcesRepository _repository;
        private readonly IEventProcessor _processor;
        
        public AddResourceHandler(IResourcesRepository repository, IEventProcessor processor)
        {
            _repository = repository;
            _processor = processor;
        }

        public async Task HandleAsync(AddResource command)
        {
            if (await _repository.ExistsAsync(command.ResourceId))
            {
                throw new ResourceAlreadyExistsException(command.ResourceId);
            }

            var resource = Resource.Create(command.ResourceId, command.Tags);
            await _repository.AddAsync(resource);
            
            await _processor.ProcessAsync(resource.Events);
        }
    }
}