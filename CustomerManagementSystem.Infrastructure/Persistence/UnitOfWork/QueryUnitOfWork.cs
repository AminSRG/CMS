using CustomerManagementSystem.Application.Customer.Interfaces;
using CustomerManagementSystem.Application.Interfaces;

namespace CustomerManagementSystem.Infrastructure.Persistence.UnitOfWork
{

    public class QueryUnitOfWork : IQueryUnitOfWork
    {
        public ICustomerQueryRepository CustomerQueryRepository { get; }
        public bool IsDisposed { get; set; }

        public QueryUnitOfWork(ICustomerQueryRepository customerQueryRepository)
        {
            CustomerQueryRepository = customerQueryRepository;
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

            IsDisposed = true;
        }

        public static void Save()
        {

        }
    }
}


