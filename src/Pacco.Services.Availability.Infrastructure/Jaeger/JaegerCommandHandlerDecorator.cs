using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using OpenTracing;
using OpenTracing.Tag;
using Pacco.Services.Availability.Application.Commands;

namespace Pacco.Services.Availability.Infrastructure.Jaeger
{
    internal class JaegerCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : class, ICommand
    {
        private ICommandHandler<TCommand> _handler { get; }
        private ITracer _tracer { get; }

        public JaegerCommandHandlerDecorator(ICommandHandler<TCommand> handler, ITracer tracer)
        {
            _handler = handler;
            _tracer = tracer;
        }

        public async Task HandleAsync(TCommand command)
        {
            var commandName = ToUnderscoreCase(command.GetType().Name);
            using (var scope = BuildScope(commandName))
            {
                var span = scope.Span;

                try
                {
                    span.Log($"Handling a message: {commandName}");
                    await _handler.HandleAsync(command);
                    span.Log($"Handled a message: {commandName}");
                }
                catch (Exception e)
                {
                    span.Log(e.Message);
                    span.SetTag(Tags.Error, true);
                    throw;
                }
            }
        }

        private IScope BuildScope(string commandName)
            => _tracer
                .BuildSpan($"handling-{commandName}")
                .WithTag("message-type", commandName)
                .AddReference(References.ChildOf, _tracer.ActiveSpan.Context)
                .StartActive(true);

        private static string ToUnderscoreCase(string str)
            => string
                .Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()))
                .ToLowerInvariant();
    }
}