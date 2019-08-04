using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Pacco.Services.Availability.Application.Exceptions;
using Pacco.Services.Availability.Application.Services;
using Pacco.Services.Availability.Core.Repositories;

namespace Pacco.Services.Availability.Application.Commands.Handlers
{
    internal sealed class ReleaseResourceHandler : ICommandHandler<ReleaseResource>
    {
        private readonly IResourcesRepository _repository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;
        private readonly ILogger<ReleaseResourceHandler> _logger;

        public ReleaseResourceHandler(IResourcesRepository repository, IMessageBroker messageBroker,
            IEventMapper eventMapper, ILogger<ReleaseResourceHandler> logger)
        {
            _repository = repository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
            _logger = logger;
        }
        
        public async Task HandleAsync(ReleaseResource command)
        {
            var resource = await _repository.GetAsync(command.ResourceId);
            
            if (resource is null)
            {
                throw new ResourceNotFoundException(command.ResourceId);
            }

            var reservation = resource.Reservations.FirstOrDefault(r => r.DateTime.Date == command.DateTime.Date);
            resource.ReleaseReservation(reservation);
            await _repository.UpdateAsync(resource);
            var events = _eventMapper.MapAll(resource.Events);
            await _messageBroker.PublishAsync(events.ToArray());
            _logger.LogInformation($"Released a resource with id: {command.ResourceId}.");
        }
    }
}