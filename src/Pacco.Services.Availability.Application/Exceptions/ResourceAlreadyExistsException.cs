using System;
using Pacco.Services.Availability.Core.Exceptions;

namespace Pacco.Services.Availability.Application.Exceptions
{
    public class ResourceAlreadyExistsException : ApplicationException
    {
        public override string Code => "resource_already_exists";
        public Guid Id { get; }

        public ResourceAlreadyExistsException(Guid id) : base($"Resource with id: {id} already exists.")
            => Id = id;
    }
}