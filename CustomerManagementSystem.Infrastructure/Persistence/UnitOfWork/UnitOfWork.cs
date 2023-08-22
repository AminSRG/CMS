using CustomerManagementSystem.Application.Customer.Interfaces;
using CustomerManagementSystem.Application.Interfaces;

namespace CustomerManagementSystem.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICustomerRepository CustomerRepository { get; }

        public bool IsDisposed { get; set; }

        public UnitOfWork(
            ICustomerRepository customerRepository)
        {
            CustomerRepository = customerRepository;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
            {
                return;
            }

            if (disposing)
            {

            }

            // Dispose unmanaged resources

            IsDisposed = true;
        }

        public Task SaveAsync()
        {
            return Task.CompletedTask;
        }
    }
}


