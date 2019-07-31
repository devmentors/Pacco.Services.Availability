using Convey.MessageBrokers;
using Pacco.Services.Availability.Application;

namespace Pacco.Services.Availability.Infrastructure.Contexts
{
    public class AppContextFactory : IAppContextFactory
    {
        private static readonly AppContext EmptyContext = new AppContext();
        private readonly ICorrelationContextAccessor _contextAccessor;

        public AppContextFactory(ICorrelationContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public IAppContext Create()
            => _contextAccessor.CorrelationContext is null
                ? EmptyContext
                : new AppContext(_contextAccessor.CorrelationContext as CorrelationContext);
    }
}