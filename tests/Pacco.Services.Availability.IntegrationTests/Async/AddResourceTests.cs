using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Pacco.Services.Availability.Api;
using Pacco.Services.Availability.Application.Commands;
using Pacco.Services.Availability.Application.Events;
using Pacco.Services.Availability.Infrastructure.Mongo.Documents;
using Pacco.Services.Availability.IntegrationTests.Fixtures;
using Shouldly;
using Xunit;

namespace Pacco.Services.Availability.IntegrationTests.Async
{
    public class AddResourceTests : IDisposable
    {
        Task Act(AddResource command)
            => _rabbitMqFixture.PublishAsync(command);
        
        [Fact]
        public async Task AddResource_Endpoint_Should_Add_Resource_With_Given_Id_To_Database()
        {
            var command = new AddResource(new Guid(ResourceId));

            var tcs = await _rabbitMqFixture.SubscribeAndGetAsync<ResourceAdded, ResourceDocument>(_mongoDbFixture.GetAsync,
                command.ResourceId);

            await Act(command);

            var document = await tcs.Task;
            
            document.ShouldNotBeNull();
            document.Id.ShouldBe(command.ResourceId);
        }
        
        #region ARRANGE    
        
        private readonly MongoDbFixture<ResourceDocument, Guid> _mongoDbFixture;
        private readonly RabbitMqFixture _rabbitMqFixture;
        private readonly HttpClient _httpClient;
        
        private const string ResourceId = "587acaf9-629f-4896-a893-4e94ae628652";

        private HttpContent GetHttpContent(AddResource command)
        {
            var json = JsonConvert.SerializeObject(command);
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return byteContent;
        }

        public AddResourceTests()
        {
            _rabbitMqFixture = new RabbitMqFixture("availability");
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