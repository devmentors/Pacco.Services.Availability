using System;
using System.Threading.Tasks;
using Convey.HTTP;
using Pacco.Services.Availability.Application.DTO;
using Pacco.Services.Availability.Application.Services.Clients;

namespace Pacco.Services.Availability.Infrastructure.Services.Clients
{
    internal sealed class CustomersServiceClient : ICustomersServiceClient
    {
        private readonly IHttpClient _httpClient;

        public CustomersServiceClient(IHttpClient httpClient)
            => _httpClient = httpClient;

        public Task<CustomerStateDto> GetStateAsync(Guid customerId)
            => _httpClient.GetAsync<CustomerStateDto>($"http://localhost:5002/customers/{customerId}​​​​​​​​/state");
    }
}