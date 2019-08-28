using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.Extensions.Logging;
using Pacco.Services.Availability.Application.Commands;
using SmartFormat;

namespace Pacco.Services.Availability.Infrastructure.Logging
{
    internal sealed class CommandHandlerLoggingDecorator<TCommand> : ICommandHandler<TCommand> 
        where TCommand : class, ICommand
    {
        private readonly ICommandHandler<TCommand> _handler;
        private readonly ILogger<TCommand> _logger;
        
        private static IReadOnlyDictionary<Type, HandlerLogMessage> MessageTemplates 
            => new Dictionary<Type, HandlerLogMessage>
        {
            {typeof(AddResource),     new HandlerLogMessage { After = "Added a resource with id: {ResourceId}. "}},
            {typeof(DeleteResource),  new HandlerLogMessage { After = "Deleted a resource with id: {ResourceId}."}},
            {typeof(ReleaseResource), new HandlerLogMessage { After = "Released a resource with id: {ResourceId}."}},
            {typeof(ReserveResource), new HandlerLogMessage { After = "Reserved a resource with id: {ResourceId} " +
                                                                      "priority: {Priority}, date: {DateTime}."}},
        };

        public CommandHandlerLoggingDecorator(ICommandHandler<TCommand> handler, ILogger<TCommand> logger)
        {
            _handler = handler;
            _logger = logger;
        }

        public async Task HandleAsync(TCommand command)
        {
            await _handler.HandleAsync(command);
            Log(command);
        }

        private void Log(TCommand command)
        {
            if(!MessageTemplates.TryGetValue(command.GetType(), out var message))
            {
                return;
            }
            
            _logger.LogInformation(Smart.Format(message.After, command));
        }
    }
}