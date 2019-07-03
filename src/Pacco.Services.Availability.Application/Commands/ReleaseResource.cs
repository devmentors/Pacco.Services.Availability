using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Availability.Application.Commands
{
    public class ReleaseResource : ICommand
    {
        public Guid Id { get; }
        public DateTime DateTime { get; }

        public ReleaseResource(Guid id, DateTime dateTime)
        {
            Id = id;
            DateTime = dateTime;
        }
    }
}