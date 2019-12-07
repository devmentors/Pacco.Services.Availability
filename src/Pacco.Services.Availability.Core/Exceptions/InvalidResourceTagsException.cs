namespace Pacco.Services.Availability.Core.Exceptions
{
    public class InvalidResourceTagsException : ExceptionBase
    {
        public override string Code => "invalid_resource_tags";
        
        public InvalidResourceTagsException() : base("Resource tags are invalid.")
        {
        }
    }
}