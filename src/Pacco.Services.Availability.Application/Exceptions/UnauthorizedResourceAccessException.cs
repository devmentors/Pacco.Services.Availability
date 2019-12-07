using System;
using Pacco.Services.Availability.Core.Exceptions;

namespace Pacco.Services.Availability.Application.Exceptions
{
    public class UnauthorizedResourceAccessException : ExceptionBase
    {
        public override string Code => "unauthorized_resource_access";
        public Guid ResourceId { get; }
        public Guid CustomerId { get; }

        public UnauthorizedResourceAccessException(Guid resourceId, Guid customerId)
            : base($"Unauthorized access to resource: '{resourceId}' by customer: '{customerId}'")
            => (ResourceId, CustomerId) = (resourceId, customerId);
    }
}