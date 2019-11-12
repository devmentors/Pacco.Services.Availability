using System.Threading.Tasks;
using Pacco.Services.Availability.Core.Events;

namespace Pacco.Services.Availability.Application.Events.Domain
{
    public interface IDomainEventHandler<in T> where T : class, IDomainEvent
    {
        Task HandleAsync(T @event);
    }
}