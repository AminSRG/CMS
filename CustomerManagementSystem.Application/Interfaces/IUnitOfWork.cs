using CustomerManagementSystem.Application.Customer.Interfaces;

namespace CustomerManagementSystem.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository CustomerRepository { get; }
    }
}


