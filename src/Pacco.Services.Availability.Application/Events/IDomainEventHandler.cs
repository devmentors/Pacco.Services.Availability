using System.Threading.Tasks;
using Pacco.Services.Availability.Core.Events;

namespace Pacco.Services.Availability.Application.Events
{
    public interface IDomainEventHandler<in TEvent> where TEvent : class, IDomainEvent
    {
        Task HandleAsync(TEvent @event);
    }
}