using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Pacco.Services.Availability.Application.Exceptions;
using Pacco.Services.Availability.Application.Services;
using Pacco.Services.Availability.Core.Entities;
using Pacco.Services.Availability.Core.Repositories;

namespace Pacco.Services.Availability.Application.Commands.Handlers
{
    internal sealed class AddResourceHandler : ICommandHandler<AddResource>
    {
        private readonly IResourcesRepository _repository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;

        public AddResourceHandler(IResourcesRepository repository, IMessageBroker messageBroker,
            IEventMapper eventMapper)
        {
            _repository = repository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }
        
        public async Task HandleAsync(AddResource command)
        {
            if (await _repository.ExistsAsync(command.Id))
            {
                throw new ResourceAlreadyExistsException(command.Id);
            }
            
            var resource = Resource.Create(command.Id);
            await _repository.AddAsync(resource);

            var events = _eventMapper.MapAll(resource.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }
}