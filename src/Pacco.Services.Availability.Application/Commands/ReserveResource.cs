using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Availability.Application.Commands
{
    public class ReserveResource : ICommand
    {
        public Guid Id { get; }
        public DateTime DateTime { get; set; }
        public Guid CustomerId { get; set; }
        public Guid OrderId { get; set; }
        public bool BelongsToVip { get; set; }

        public ReserveResource(Guid id, DateTime dateTime, Guid customerId, Guid orderId, 
            bool belongsToVip)
        {
            Id = id;
            DateTime = dateTime;
            CustomerId = customerId;
            OrderId = orderId;
            BelongsToVip = belongsToVip;
        }
    }
}