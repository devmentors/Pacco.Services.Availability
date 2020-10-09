using System.Threading.Tasks;
using Convey.CQRS.Events;

namespace Pacco.Services.Availability.Application.Events.External.Handlers
{
    public sealed class CustomerCreatedHandler : IEventHandler<CustomerCreated>
    {
        public async Task HandleAsync(CustomerCreated @event)
        {
            // add customer to mongo
        }
    }
}