using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Pacco.Services.Availability.Application.Exceptions;
using Pacco.Services.Availability.Application.Services;
using Pacco.Services.Availability.Core.Repositories;
using Pacco.Services.Availability.Core.ValueObjects;

namespace Pacco.Services.Availability.Application.Commands.Handlers
{
    public class ReserveResourceHandler : ICommandHandler<ReserveResource>
    {
        private readonly IResourcesRepository _resourcesRepository;
        private readonly IEventProcessor _eventProcessor;

        public ReserveResourceHandler(IResourcesRepository resourcesRepository, IEventProcessor eventProcessor)
        {
            _resourcesRepository = resourcesRepository;
            _eventProcessor = eventProcessor;
        }
        
        public async Task HandleAsync(ReserveResource command)
        {
            var resource = await _resourcesRepository.GetAsync(command.ResourceId);
            if (resource is null)
            {
                throw new ResourceNotFoundException(command.ResourceId);
            }
            
            var reservation = new Reservation(command.DateTime, command.Priority);
            resource.AddReservation(reservation);
            await _resourcesRepository.UpdateAsync(resource);
            await _eventProcessor.ProcessAsync(resource.Events);
        }
    }
}