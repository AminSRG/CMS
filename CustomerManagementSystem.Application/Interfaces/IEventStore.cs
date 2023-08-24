using CustomerManagementSystem.Domain.Event;

namespace CustomerManagementSystem.Application.Interfaces
{
    public interface IEventStore
    {
        Task AppendEventsAsync(string streamName, List<CustomerEvent> events);
        Task<List<CustomerEvent>> GetEventsAsync(string streamName);
    }

}
