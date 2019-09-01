using System;
using System.Threading.Tasks;
using Pacco.Services.Availability.Application.DTO;

namespace Pacco.Services.Availability.Application.Services.Clients
{
    public interface ICustomersServiceClient
    {
        Task<CustomerStateDto> GetStateAsync(Guid id);
    }
}