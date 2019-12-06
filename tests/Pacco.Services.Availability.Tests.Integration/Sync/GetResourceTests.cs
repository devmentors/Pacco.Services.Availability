using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Pacco.Services.Availability.Api;
using Pacco.Services.Availability.Application.DTO;
using Pacco.Services.Availability.Infrastructure.Mongo.Documents;
using Pacco.Services.Availability.Tests.Integration.Fixtures;
using Shouldly;
using Xunit;

namespace Pacco.Services.Availability.Tests.Integration.Sync
{
    public class GetResourceTests : IDisposable
    {
        Task<HttpResponseMessage> Act()
            => _httpClient.GetAsync($"resources/{ResourceId}");

        [Fact]
        public async Task GetResource_Endpoint_Should_Return_NotFound_Http_Status_Code_If_No_Resource_Is_Found()
        {
            var response = await Act();

            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
        
        [Fact]
        public async Task GetResource_Endpoint_Should_Return_Success_Http_Status_Code_If_Resource_Is_Found()
        {
            await InsertResourceAsync();
            
            var response = await Act();
           
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
        
        [Fact]
        public async Task GetResource_Endpoint_Should_Return_Dto_With_Correct_Data()
        {
            await InsertResourceAsync();
            
            var response = await Act();
            var dto = await GetDtoAsync(response);
            var reservations = dto.Reservations.ToArray(); 
            
            dto.ShouldNotBeNull();
            dto.Id.ToString().ShouldBe(ResourceId);
            reservations.Length.ShouldBe(1);
            reservations.First().Priority.ShouldBe(Reservation.Priority);
            reservations.First().DateTime.ShouldBe(Reservation.TimeStamp.AsDateTime());
        }
        
        #region ARRANGE    
        
        private readonly MongoDbFixture<ResourceDocument, Guid> _mongoDbFixture;
        private readonly HttpClient _httpClient;
        
        private const string ResourceId = "587acaf9-629f-4896-a893-4e94ae628652";
        private readonly ReservationDocument Reservation = new ReservationDocument
        {
            Priority = 1, TimeStamp = DateTime.UtcNow.AsDaysSinceEpoch()
        };
        
        private async Task InsertResourceAsync()
        {
            var resourceId = new Guid(ResourceId);
            await _mongoDbFixture.InsertAsync(new ResourceDocument
            {
                Id = resourceId, 
                Reservations = new List<ReservationDocument>
                {
                    Reservation
                }
            });
        }

        private async Task<ResourceDto> GetDtoAsync(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ResourceDto>(json);
        }
        
        public GetResourceTests()
        {
            _mongoDbFixture = new MongoDbFixture<ResourceDocument, Guid>("resource-test-db", 
                "Resources");

            var server = new TestServer(Program.GetWebHostBuilder(new string[]{}));
            _httpClient = server.CreateClient();
        }
        
        public void Dispose()
        {
            _mongoDbFixture.Dispose();
        }
        #endregion
    }
}