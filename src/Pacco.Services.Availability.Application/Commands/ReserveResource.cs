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
        public bool CanExpropriate { get; }

        public ReserveResource(Guid id, DateTime dateTime, Guid customerId, Guid orderId, 
            bool belongsToVip, bool canExpropriate)
        {
            Id = id;
            DateTime = dateTime;
            CustomerId = customerId;
            OrderId = orderId;
            BelongsToVip = belongsToVip;
            CanExpropriate = canExpropriate;
        }
    }
}