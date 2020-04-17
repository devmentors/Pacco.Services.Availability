using System;
using System.Net.Http;
using System.Threading.Tasks;
using Pacco.Services.Availability.Application.DTO;
using Pacco.Services.Availability.Application.Services;

namespace Pacco.Services.Availability.Infrastructure.Services
{
    public class CustomersServiceClient : ICustomersServiceClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CustomersServiceClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        
        public Task<CustomerStateDto> GetStateAsync(Guid customerId)
        {
            var client = _httpClientFactory.CreateClient();
            throw new NotImplementedException();
        }
    }
}