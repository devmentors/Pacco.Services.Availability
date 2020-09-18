using System;
using System.Threading;
using System.Threading.Tasks;
using Pacco.Services.Availability.Api;
using Pacco.Services.Availability.Application.Commands;
using Pacco.Services.Availability.Application.Events;
using Pacco.Services.Availability.Infrastructure.Mongo.Documents;
using Pacco.Services.Availability.Tests.Shared.Factories;
using Pacco.Services.Availability.Tests.Shared.Fixtures;
using Shouldly;
using Xunit;

namespace Pacco.Services.Availability.Tests.Integration.Async
{
    public class AddResourceTests : IClassFixture<PaccoApplicationFactory<Program>>
    {
        Task act(AddResource command)
            => _rabbitMqFixture.PublishAsync(command, Exchange);

        [Fact]
        public async Task add_resource_subscription_should_insert_document_into_database()
        {
            var command = new AddResource(Guid.NewGuid(), new []{"tag"});

            var tcs = _rabbitMqFixture.SubscribeAndGet<ResourceAdded, ResourceDocument>(Exchange,
                _mongoDbFixture.GetAsync, command.ResourceId);
            
            await act(command);

            var document = await tcs.Task;

            document.Id.ShouldBe(command.ResourceId);
        }

        #region ARRANGE

        private const string Exchange = "availability";
        private readonly MongoDbFixture<ResourceDocument, Guid> _mongoDbFixture;
        private readonly RabbitMqFixture _rabbitMqFixture;
        
        public AddResourceTests(PaccoApplicationFactory<Program> factory)
        {
            _mongoDbFixture = new MongoDbFixture<ResourceDocument, Guid>("resources");
            _rabbitMqFixture = new RabbitMqFixture(Exchange);
            factory.Server.AllowSynchronousIO = true;
        }

        #endregion
    }
}