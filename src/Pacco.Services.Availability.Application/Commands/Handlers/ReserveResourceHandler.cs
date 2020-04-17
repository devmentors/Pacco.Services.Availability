using System.Net.Http;
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
        private readonly ICustomersServiceClient _customersServiceClient;

        public ReserveResourceHandler(IResourcesRepository resourcesRepository, IEventProcessor eventProcessor,
            ICustomersServiceClient customersServiceClient)
        {
            _resourcesRepository = resourcesRepository;
            _eventProcessor = eventProcessor;
            _customersServiceClient = customersServiceClient;
        }
        
        public async Task HandleAsync(ReserveResource command)
        {
            var resource = await _resourcesRepository.GetAsync(command.ResourceId);
            if (resource is null)
            {
                throw new ResourceNotFoundException(command.ResourceId);
            }

            var customerState = await _customersServiceClient.GetStateAsync(command.CustomerId);
            if (customerState is null)
            {
                throw new CustomerNotFoundException(command.CustomerId);
            }

            if (!customerState.IsValid)
            {
                throw new InvalidCustomerStateException(command.CustomerId, customerState.State);
            }
            
            var reservation = new Reservation(command.DateTime, command.Priority);
            resource.AddReservation(reservation);
            await _resourcesRepository.UpdateAsync(resource);
            await _eventProcessor.ProcessAsync(resource.Events);
        }
    }
}