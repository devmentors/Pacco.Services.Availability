using Convey;
using Convey.CQRS.Commands;
using Microsoft.Extensions.DependencyInjection;
using Pacco.Services.Availability.Application.Commands;

namespace Pacco.Services.Availability.Infrastructure.Jaeger
{
    internal static class Extensions
    {
        public static IConveyBuilder AddJaegerDecorators(this IConveyBuilder builder)
        {
            builder.Services.Decorate(typeof(ICommandHandler<ReserveResource>),
                typeof(JaegerCommandHandlerDecorator<ReserveResource>));
            return builder;
        }
    }
}