using System;
using System.Threading.Tasks;
using Pacco.Services.Availability.Application.DTO.External;

namespace Pacco.Services.Availability.Application.Services.Clients
{
    public interface ICustomersServiceClient
    {
        Task<CustomerStateDto> GetStateAsync(Guid id);
    }
}