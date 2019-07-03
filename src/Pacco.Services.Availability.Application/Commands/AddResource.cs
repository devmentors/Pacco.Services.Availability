using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Availability.Application.Commands
{
    public class AddResource : ICommand
    {
        public Guid Id { get; }

        public AddResource(Guid id)
            => Id = id;
    }
}