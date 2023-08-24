using CustomerManagementSystem.Application.Interfaces;
using CustomerManagementSystem.Domain.Event;

namespace CustomerManagementSystem.Infrastructure.Persistence.Event
{
    public class InMemoryEventBroker : IEventBroker
    {
        private readonly Dictionary<Type, List<object>> _subscriptions = new Dictionary<Type, List<object>>();

        public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : CustomerEvent
        {
            var eventType = typeof(TEvent);
            if (!_subscriptions.ContainsKey(eventType))
            {
                _subscriptions[eventType] = new List<object>();
            }

            _subscriptions[eventType].Add(handler);
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : CustomerEvent
        {
            var eventType = typeof(TEvent);
            if (_subscriptions.ContainsKey(eventType))
            {
                foreach (var handler in _subscriptions[eventType])
                {
                    ((Action<TEvent>)handler)(@event);
                }
            }
        }
    }

}
