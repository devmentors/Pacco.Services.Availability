namespace Pacco.Services.Availability.Core.Exceptions
{
    public class MissingResourceTagsException : DomainException
    {
        public override string Code { get; } = "missing_resource_tags";
        
        public MissingResourceTagsException() : base("Resource tags are missing.")
        {
        }
    }
}