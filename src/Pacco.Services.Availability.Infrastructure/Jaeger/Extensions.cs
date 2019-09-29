using Convey;

namespace Pacco.Services.Availability.Infrastructure.Jaeger
{
    internal static class Extensions
    {
        public static IConveyBuilder AddJaegerDecorators(this IConveyBuilder builder)
        {
//            builder.Services.Decorate(typeof(ICommandHandler<ReserveResource>),
//                typeof(JaegerCommandHandlerDecorator<ReserveResource>));
            return builder;
        }
    }
}