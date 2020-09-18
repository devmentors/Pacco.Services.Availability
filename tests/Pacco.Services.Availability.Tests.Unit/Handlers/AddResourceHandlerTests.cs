using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using NSubstitute;
using Pacco.Services.Availability.Application.Commands;
using Pacco.Services.Availability.Application.Commands.Handlers;
using Pacco.Services.Availability.Application.Exceptions;
using Pacco.Services.Availability.Application.Services;
using Pacco.Services.Availability.Core.Entities;
using Pacco.Services.Availability.Core.Repositories;
using Shouldly;
using Xunit;

namespace Pacco.Services.Availability.Tests.Unit.Handlers
{
    public class AddResourceHandlerTests
    {
        Task act(AddResource command)
            => _handler.HandleAsync(command);

        [Fact]
        public async Task given_id_of_resource_already_existing_the_exception_should_be_thrown()
        {
            var command = new AddResource(Guid.NewGuid(), new []{"tag"});
            _repository.ExistsAsync(command.ResourceId).Returns(true);

            var exception = await Record.ExceptionAsync(() => act(command));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ResourceAlreadyExistsException>();
        }

        [Fact]
        public async Task ginen_valid_id_and_tags_the_repository_should_receive_resource()
        {
            var command = new AddResource(Guid.NewGuid(), new []{"tag"});
            _repository.ExistsAsync(command.ResourceId).Returns(false);

            await act(command);

            await _repository.Received(1).AddAsync(Arg.Is<Resource>(r => r.Id == command.ResourceId));
        }
        
#region ARRANGE

        private readonly ICommandHandler<AddResource> _handler;
        private readonly IResourcesRepository _repository;
        private readonly IEventProcessor _eventProcessor;
        
        public AddResourceHandlerTests()
        {
            _repository = Substitute.For<IResourcesRepository>();
            _eventProcessor = Substitute.For<IEventProcessor>();
            _handler = new AddResourceHandler(_repository, _eventProcessor);
        }

#endregion
    }
}