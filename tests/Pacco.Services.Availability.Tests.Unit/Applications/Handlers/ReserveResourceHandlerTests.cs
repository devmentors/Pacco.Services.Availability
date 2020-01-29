using System;
using System.Threading.Tasks;
using NSubstitute;
using Pacco.Services.Availability.Application;
using Pacco.Services.Availability.Application.Commands;
using Pacco.Services.Availability.Application.Commands.Handlers;
using Pacco.Services.Availability.Application.DTO;
using Pacco.Services.Availability.Application.Exceptions;
using Pacco.Services.Availability.Application.Services;
using Pacco.Services.Availability.Application.Services.Clients;
using Pacco.Services.Availability.Core.Entities;
using Pacco.Services.Availability.Core.Repositories;
using Shouldly;
using Xunit;

namespace Pacco.Services.Availability.Tests.Unit.Applications.Handlers
{
    public class ReserveResourceHandlerTests
    {
        private Task Act(ReserveResource command) => _handler.HandleAsync(command);

        [Fact]
        public async Task given_invalid_resource_id_reserve_resource_should_throw_an_exception()
        {
            var command = new ReserveResource(Guid.NewGuid(), DateTime.UtcNow, 0, Guid.NewGuid());
            var exception = await Record.ExceptionAsync(async () => await Act(command));
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ResourceNotFoundException>();
        }

        [Fact]
        public async Task given_valid_resource_id_for_valid_customer_reserve_resource_should_succeed()
        {
            var command = new ReserveResource(Guid.NewGuid(), DateTime.UtcNow, 0, Guid.NewGuid());
            var resource = Resource.Create(new AggregateId(), new[] {"tag"});
            _resourcesRepository.GetAsync(command.ResourceId).Returns(resource);

            var customerState = new CustomerStateDto
            {
                State = "valid"
            };
            _customersServiceClient.GetStateAsync(command.CustomerId).Returns(customerState);

            await Act(command);

            await _resourcesRepository.Received().UpdateAsync(resource);
            await _eventProcessor.Received().ProcessAsync(resource.Events);
        }

        #region Arrange

        private readonly ReserveResourceHandler _handler;
        private readonly IResourcesRepository _resourcesRepository;
        private readonly ICustomersServiceClient _customersServiceClient;
        private readonly IEventProcessor _eventProcessor;
        private readonly IAppContext _appContext;
        
        public ReserveResourceHandlerTests()
        {
            _resourcesRepository = Substitute.For<IResourcesRepository>();
            _customersServiceClient = Substitute.For<ICustomersServiceClient>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _appContext = Substitute.For<IAppContext>();
            _handler = new ReserveResourceHandler(_resourcesRepository, _customersServiceClient, _eventProcessor,
                _appContext);
        }

        #endregion
    }
}