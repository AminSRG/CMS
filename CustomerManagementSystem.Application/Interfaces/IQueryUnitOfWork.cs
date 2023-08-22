using CustomerManagementSystem.Application.Customer.Interfaces;

namespace CustomerManagementSystem.Application.Interfaces
{
    public interface IQueryUnitOfWork : IDisposable
    {
        ICustomerQueryRepository CustomerQueryRepository { get; }
    }
}


