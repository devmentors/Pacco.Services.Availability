using System;
using Pacco.Services.Availability.Core.Exceptions;

namespace Pacco.Services.Availability.Application.Exceptions
{
    public class ResourceNotFoundException : ApplicationException
    {
        public override string Code => "resource_not_found";
        public Guid Id { get; }

        public ResourceNotFoundException(Guid id) : base($"Resource with id: {id} was not found.")
            => Id = id;
    }
}