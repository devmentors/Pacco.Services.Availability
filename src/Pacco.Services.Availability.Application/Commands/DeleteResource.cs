using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Availability.Application.Commands
{
    public class DeleteResource : ICommand
    {
        public Guid Id { get; }

        public DeleteResource(Guid id)
            => Id = id;
    }
}