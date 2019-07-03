using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Pacco.Services.Availability.Application.Exceptions;
using Pacco.Services.Availability.Application.Services;
using Pacco.Services.Availability.Core.Repositories;
using Pacco.Services.Availability.Core.ValueObjects;

namespace Pacco.Services.Availability.Application.Commands.Handlers
{
    internal sealed class ReserveResourceHandler : ICommandHandler<ReserveResource>
    {
        private readonly IResourcesRepository _repository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;

        public ReserveResourceHandler(IResourcesRepository repository, IMessageBroker messageBroker,
            IEventMapper eventMapper)
        {
            _repository = repository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }
        
        public async Task HandleAsync(ReserveResource command)
        {
            var resource = await _repository.GetAsync(command.Id);
            
            if (resource is null)
            {
                throw new ResourceNotFoundException(command.Id);
            }

            var reservation = new Reservation(command.DateTime, command.CustomerId, 
                command.OrderId, command.BelongsToVip);
            
            resource.AddReservation(reservation, command.CanExpropriate);

            await _repository.UpdateAsync(resource);

            var events = _eventMapper.MapAll(resource.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }
}