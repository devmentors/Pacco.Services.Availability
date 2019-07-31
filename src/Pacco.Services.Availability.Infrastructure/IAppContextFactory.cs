using Pacco.Services.Availability.Application;

namespace Pacco.Services.Availability.Infrastructure
{
    public interface IAppContextFactory
    {
        IAppContext Create();
    }
}