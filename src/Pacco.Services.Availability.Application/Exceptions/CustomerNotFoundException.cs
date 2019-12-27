using System;
using Pacco.Services.Availability.Core.Exceptions;

namespace Pacco.Services.Availability.Application.Exceptions
{
    public class CustomerNotFoundException : ApplicationException
    {
        public override string Code => "customer_not_found";
        public Guid Id { get; }

        public CustomerNotFoundException(Guid id) : base($"Customer with id: {id} was not found.")
            => Id = id;
    }
}