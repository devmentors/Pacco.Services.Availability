using System.Threading.Tasks;
using Convey.CQRS.Commands;
using OpenTracing;

namespace Pacco.Services.Availability.Infrastructure.Jaeger
{
    internal sealed class JaegerCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : class, ICommand
    {
        private readonly ICommandHandler<TCommand> _handler;
        private readonly ITracer _tracer;

        public JaegerCommandHandlerDecorator(ICommandHandler<TCommand> handler, ITracer tracer)
        {
            _handler = handler;
            _tracer = tracer;
        }

        public async Task HandleAsync(TCommand command)
        {
            await _handler.HandleAsync(command);
        }
    }
}