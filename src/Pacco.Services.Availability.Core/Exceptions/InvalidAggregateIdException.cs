namespace Pacco.Services.Orders.Core.Exceptions
{
    public class InvalidAggregateIdException : ExceptionBase
    {
        public override string Code => "invalid_aggregate_id";
        
        public InvalidAggregateIdException() : base($"Invalid aggregate id.")
        {
        }
    }
}