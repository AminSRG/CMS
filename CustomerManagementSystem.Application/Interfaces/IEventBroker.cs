using CustomerManagementSystem.Domain.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagementSystem.Application.Interfaces
{
    public interface IEventBroker
    {
        void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : CustomerEvent;
        void Publish<TEvent>(TEvent @event) where TEvent : CustomerEvent;
    }

}
