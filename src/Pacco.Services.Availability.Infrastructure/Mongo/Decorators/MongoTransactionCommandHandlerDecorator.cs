using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.Persistence.MongoDB;

namespace Pacco.Services.Availability.Infrastructure.Mongo.Decorators
{
    internal sealed class MongoTransactionCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> 
        where TCommand : class, ICommand
    {
        private readonly ICommandHandler<TCommand> _handler;
        private readonly IMongoSessionFactory _sessionFactory;
        
        public MongoTransactionCommandHandlerDecorator(ICommandHandler<TCommand> handler, IMongoSessionFactory sessionFactory)
        {
            _handler = handler;
            _sessionFactory = sessionFactory;
        }
        
        public async Task HandleAsync(TCommand command)
        {
            using var session = await _sessionFactory.CreateAsync();
            session.StartTransaction();
            
            try
            {
                await _handler.HandleAsync(command);
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