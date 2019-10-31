using Pacco.Services.Availability.Application;

namespace Pacco.Services.Availability.Infrastructure.Contexts
{
    internal interface IAppContextFactory
    {
        IAppContext Create();
    }
}