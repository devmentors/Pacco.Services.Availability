using System.Collections.Generic;
using System.Threading.Tasks;
using Pacco.Services.Availability.Core.Events;

namespace Pacco.Services.Availability.Application.Services
{
    public interface IEventProcessor
    {
        Task ProcessAsync(IEnumerable<IDomainEvent> events);
    }
}