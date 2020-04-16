using System;

namespace Pacco.Services.Availability.Application.Exceptions
{
    public class ResourceAlreadyExistsException : AppException
    {
        public Guid Id { get; }
        public override string Code => "resource_already_exists";

        public ResourceAlreadyExistsException(Guid id) : base($"Resource with ID: '{id}' already exists.")
        {
            Id = id;
        }
    }
}