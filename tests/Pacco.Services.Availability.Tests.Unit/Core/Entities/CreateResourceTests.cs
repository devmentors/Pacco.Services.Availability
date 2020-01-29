using System.Collections.Generic;
using System.Linq;
using Pacco.Services.Availability.Core.Entities;
using Pacco.Services.Availability.Core.Events;
using Pacco.Services.Availability.Core.Exceptions;
using Shouldly;
using Xunit;

namespace Pacco.Services.Availability.Tests.Unit.Core.Entities
{
    public class CreateResourceTests
    {
        private Resource Act(AggregateId id, IEnumerable<string> tags) => Resource.Create(id, tags);

        [Fact]
        public void given_valid_id_and_tags_resource_should_be_created()
        {
            // Arrange
            var id = new AggregateId();
            var tags = new[] {"tag"};

            // Act
            var resource = Act(id, tags);
            
            // Assert
            resource.ShouldNotBeNull();
            resource.Id.ShouldBe(id);
            resource.Tags.ShouldBe(tags);
            resource.Events.Count().ShouldBe(1);

            var @event = resource.Events.Single();
            @event.ShouldBeOfType<ResourceCreated>();
        }

        [Fact]
        public void given_empty_tags_resource_should_throw_an_exception()
        {
            var id = new AggregateId();
            var tags = Enumerable.Empty<string>();

            var exception = Record.Exception(() => Act(id, tags));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<MissingResourceTagsException>();
        }
    }
}