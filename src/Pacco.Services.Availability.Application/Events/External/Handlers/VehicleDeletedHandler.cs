using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Microsoft.Extensions.Logging;
using Pacco.Services.Availability.Application.Commands;

namespace Pacco.Services.Availability.Application.Events.External.Handlers
{
    public class VehicleDeletedHandler : IEventHandler<VehicleDeleted>
    {
        private readonly ICommandDispatcher _dispatcher;
        private readonly ILogger<VehicleDeletedHandler> _logger;

        public VehicleDeletedHandler(ICommandDispatcher dispatcher, ILogger<VehicleDeletedHandler> logger)
        {
            _dispatcher = dispatcher;
            _logger = logger;
        }

        public Task HandleAsync(VehicleDeleted @event)
        {
            _logger.LogInformation($"Vehicle with id: {@event.VehicleId} has been deleted.");
            return _dispatcher.SendAsync(new DeleteResource(@event.VehicleId));
        }
    }
}