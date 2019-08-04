using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Pacco.Services.Availability.Application.Exceptions;
using Pacco.Services.Availability.Application.Services;
using Pacco.Services.Availability.Core.Repositories;

namespace Pacco.Services.Availability.Application.Commands.Handlers
{
    internal sealed class DeleteResourceHandler : ICommandHandler<DeleteResource>
    {
        private readonly IResourcesRepository _repository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;
        private readonly ILogger<DeleteResourceHandler> _logger;

        public DeleteResourceHandler(IResourcesRepository repository, IMessageBroker messageBroker,
            IEventMapper eventMapper, ILogger<DeleteResourceHandler> logger)
        {
            _repository = repository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
            _logger = logger;
        }
        
        public async Task HandleAsync(DeleteResource command)
        {
            var resource = await _repository.GetAsync(command.ResourceId);
            
            if (resource is null)
            {
                throw new ResourceNotFoundException(command.ResourceId);
            }

            resource.Delete();
            await _repository.DeleteAsync(resource.Id);
            var events = _eventMapper.MapAll(resource.Events);
            await _messageBroker.PublishAsync(events.ToArray());
            _logger.LogInformation($"Deleted a resource with id: {command.ResourceId}.");
        }
    }
}