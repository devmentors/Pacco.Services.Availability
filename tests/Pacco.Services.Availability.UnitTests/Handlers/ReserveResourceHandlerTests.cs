using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using NSubstitute;
using Pacco.Services.Availability.Application.Commands;
using Pacco.Services.Availability.Application.Commands.Handlers;
using Pacco.Services.Availability.Application.Events;
using Pacco.Services.Availability.Application.Exceptions;
using Pacco.Services.Availability.Application.Services;
using Pacco.Services.Availability.Core.Entities;
using Pacco.Services.Availability.Core.Events;
using Pacco.Services.Availability.Core.Repositories;
using Shouldly;
using Xunit;

namespace Pacco.Services.Availability.UnitTests.Handlers
{
    public class ReserveResourceHandlerTests
    {
        Task Act(ReserveResource command)
            => _handler.HandleAsync(command);

        [Fact]
        public async Task HandleAsync_Should_Throw_If_Resource_Is_Not_Found()
        {
            //ARRANGE
            var command = new ReserveResource(Guid.Empty, DateTime.Now, 1);

            var exception = await Record.ExceptionAsync(async() => await Act(command));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeAssignableTo<ResourceNotFoundException>();
            ((ResourceNotFoundException)exception).Id.ShouldBe(command.Id);
        }
        
        [Fact]
        public async Task HandleAsync_Should_AddReservation_And_Publish_Events()
        {
            //ARRANGE

            var resourceId = Guid.NewGuid();
            var resource = Resource.Create(resourceId);
            var command = new ReserveResource(resourceId, DateTime.Now, 1);

            _repository.GetAsync(resourceId).Returns(resource);
            _mapper.MapAll(null).ReturnsForAnyArgs(new List<IEvent>
            {
                new ResourceReserved(command.Id, command.DateTime)
            });

            await Act(command);

            await _repository
                .Received()
                .UpdateAsync(resource);

            await _broker
                .Received()
                .PublishAsync(Arg.Any<ResourceReserved>());
        }

        #region  ARRANGE

        private readonly ICommandHandler<ReserveResource> _handler;
        private readonly IResourcesRepository _repository;
        private readonly IEventMapper _mapper;
        private readonly IMessageBroker _broker;
        
        //[Setup]
        public ReserveResourceHandlerTests()
        {
            _repository = Substitute.For<IResourcesRepository>();
            _mapper = Substitute.For<IEventMapper>();
            _broker = Substitute.For<IMessageBroker>();
            _handler = new ReserveResourceHandler(_repository, _broker, _mapper);
        } 

        #endregion
    }
}