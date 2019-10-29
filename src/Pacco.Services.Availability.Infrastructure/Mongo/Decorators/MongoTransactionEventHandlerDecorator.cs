using System;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Convey.Persistence.MongoDB;

namespace Pacco.Services.Availability.Infrastructure.Mongo.Decorators
{
    public class MongoTransactionEventHandlerDecorator<TEvent> : IEventHandler<TEvent> where TEvent : class, IEvent
    {
        private readonly IEventHandler<TEvent> _handler;
        private readonly IMongoSessionFactory _sessionFactory;
        
        public MongoTransactionEventHandlerDecorator(IEventHandler<TEvent> handler, IMongoSessionFactory sessionFactory)
        {
            _handler = handler;
            _sessionFactory = sessionFactory;
        }
        
        public async Task HandleAsync(TEvent @event)
        {
            using var session = await _sessionFactory.CreateAsync();
            session.StartTransaction();
            
            try
            {
                await _handler.HandleAsync(@event);
                await session.CommitTransactionAsync();
            }
            catch (Exception e)
            {
                await session.AbortTransactionAsync();
                throw;
            }
        }
    }
}