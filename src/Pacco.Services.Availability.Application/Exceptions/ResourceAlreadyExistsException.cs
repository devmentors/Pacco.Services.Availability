using System;

namespace Pacco.Services.Availability.Application.Exceptions
{
    public class ResourceAlreadyExistsException : AppException
    {
        public Guid ResourceId { get; }
        public override string Code => "resource_already_exists";
            
        public ResourceAlreadyExistsException(Guid resourceId) 
            : base($"Resource {resourceId} already exists")
        {
            ResourceId = resourceId;
        }
    }
}