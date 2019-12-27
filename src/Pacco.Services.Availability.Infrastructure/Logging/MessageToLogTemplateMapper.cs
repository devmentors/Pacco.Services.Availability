using System;
using System.Collections.Generic;
using Convey.Logging.CQRS;
using Pacco.Services.Availability.Application.Commands;
using Pacco.Services.Availability.Application.Events.External;
using Pacco.Services.Availability.Application.Exceptions;

namespace Pacco.Services.Availability.Infrastructure.Logging
{
    internal sealed class MessageToLogTemplateMapper : IMessageToLogTemplateMapper
    {
        private static IReadOnlyDictionary<Type, HandlerLogTemplate> MessageTemplates 
            => new Dictionary<Type, HandlerLogTemplate>
            {
                {
                    typeof(AddResource),     
                    new HandlerLogTemplate
                    {
                        After = "Added a resource with id: {ResourceId}. ",
                        OnError = new Dictionary<Type, string>
                        {
                            { typeof(ResourceAlreadyExistsException), "Resource with id: {ResourceId} already exists."}
                        }
                    }
                },
                {typeof(DeleteResource),  new HandlerLogTemplate { After = "Deleted a resource with id: {ResourceId}."}},
                {typeof(ReleaseResourceReservation), new HandlerLogTemplate { After = "Released a resource with id: {ResourceId}."}},
                {typeof(ReserveResource), new HandlerLogTemplate { After = "Reserved a resource with id: {ResourceId} " +
                                                                          "priority: {Priority}, date: {DateTime}."}},
                {typeof(VehicleDeleted), new HandlerLogTemplate{ Before = "Vehicle with id: {VehicleId} has been deleted."}}, 
            };
        
        public HandlerLogTemplate Map<TMessage>(TMessage message) where TMessage : class
        {
            var key = message.GetType();
            return MessageTemplates.TryGetValue(key, out var template) ? template : null;
        }
    }
}