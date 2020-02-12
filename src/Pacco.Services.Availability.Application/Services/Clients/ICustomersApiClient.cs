using System;
using System.Threading.Tasks;
using Pacco.Services.Availability.Application.DTO;

namespace Pacco.Services.Availability.Application.Services.Clients
{
    public interface ICustomersApiClient
    {
        Task<CustomerStateDto> GetStateAsync(Guid customerId);
    }
}