using CustomerManagementSystem.Application.Interfaces;
using CustomerManagementSystem.Domain.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagementSystem.Infrastructure.Persistence.Event
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly Dictionary<string, List<CustomerEvent>> _eventStore = new Dictionary<string, List<CustomerEvent>>();

        public async Task AppendEventsAsync(string streamName, List<CustomerEvent> events)
        {
            if (!_eventStore.ContainsKey(streamName))
            {
                _eventStore[streamName] = new List<CustomerEvent>();
            }

            _eventStore[streamName].AddRange(events);
        }

        public async Task<List<CustomerEvent>> GetEventsAsync(string streamName)
        {
            if (_eventStore.ContainsKey(streamName))
            {
                return _eventStore[streamName];
            }

            return new List<CustomerEvent>();
        }
    }
}
