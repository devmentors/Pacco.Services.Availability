using System.Threading.Tasks;
using Convey.CQRS.Events;

namespace Pacco.Services.Availability.Application.Events.External.Handlers
{
    public class CustomerCreatedHandler : IEventHandler<CustomerCreated>
    {
        public async Task HandleAsync(CustomerCreated @event)
        {
            // Customers -> insert new Customer()
            await Task.CompletedTask;
        }
    }
}