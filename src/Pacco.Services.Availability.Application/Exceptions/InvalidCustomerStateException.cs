using System;
using Pacco.Services.Availability.Core.Exceptions;

namespace Pacco.Services.Availability.Application.Exceptions
{
    public class InvalidCustomerStateException : ExceptionBase
    {
        public override string Code => "invalid_customer_state";
        public Guid Id { get; }
        public string State { get; }

        public InvalidCustomerStateException(Guid id, string state)
            : base($"Customer with id: {id} has invalid state: {state}.")
            => (Id, State) = (id, state);
    }
}