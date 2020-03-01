using System;
using System.Threading.Tasks;
using Convey.HTTP;
using Convey.Secrets.Vault;
using Convey.WebApi.Security;
using Pacco.Services.Availability.Application.DTO;
using Pacco.Services.Availability.Application.Services.Clients;

namespace Pacco.Services.Availability.Infrastructure.Services.Clients
{
    internal sealed class CustomersServiceClient : ICustomersServiceClient
    {
        private readonly IHttpClient _httpClient;
        private readonly string _url;

        public CustomersServiceClient(IHttpClient httpClient, HttpClientOptions options,
            ICertificatesService certificatesService, VaultOptions vaultOptions, SecurityOptions securityOptions)
        {
            _httpClient = httpClient;
            _url = options.Services["customers"];
            if (!vaultOptions.Enabled || vaultOptions.Pki?.Enabled != true)
            {
                return;
            }

            var certificate = certificatesService.Get(vaultOptions.Pki.RoleName);
            if (certificate is null)
            {
                return;
            }

            var header = securityOptions.Certificate.GetHeaderName();
            var certificateData = certificate.GetRawCertDataString();
            _httpClient.SetHeaders(h => h.Add(header, certificateData));
        }

        public Task<CustomerStateDto> GetStateAsync(Guid id)
            => _httpClient.GetAsync<CustomerStateDto>($"{_url}/customers/{id}/state");
    }
}