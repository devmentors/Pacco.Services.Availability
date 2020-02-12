using System.Reflection;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Pacco.Services.Availability.Application.Events;
using Pacco.Services.Availability.Application.Exceptions;
using Pacco.Services.Availability.Application.Services;
using Pacco.Services.Availability.Application.Services.Clients;
using Pacco.Services.Availability.Core.Entities;
using Pacco.Services.Availability.Core.Repositories;
using Pacco.Services.Availability.Core.ValueObjects;

namespace Pacco.Services.Availability.Application.Commands.Handlers
{
    public sealed class ReserveResourceHandler : ICommandHandler<ReserveResource>
    {
        private readonly IResourcesRepository _repository;
        private readonly IEventProcessor _eventProcessor;
        private readonly ICustomersApiClient _client;

        public ReserveResourceHandler(IResourcesRepository repository,
            IEventProcessor eventProcessor, ICustomersApiClient client)
        {
            _repository = repository;
            _eventProcessor = eventProcessor;
            _client = client;
        }

        public async Task HandleAsync(ReserveResource command)
        {
            var resource = await _repository.GetAsync(command.ResourceId);
            if (resource is null)
            {
                throw new ResourceNotFoundException(command.ResourceId);
            }

            var state = await _client.GetStateAsync(command.CustomerId);

            if (state is null)
            {
                throw new CustomerNotFoundException(command.CustomerId);
            }
            
            if (!state.IsValid)
            {
                throw new InvalidCustomerStateException(command.CustomerId, state.State);
            }
            
            var reservation = new Reservation(command.DateTime, command.Priority);
            resource.AddReservation(reservation);
            await _repository.UpdateAsync(resource);
            await _eventProcessor.ProcessAsync(resource.Events);
        }
    }
}