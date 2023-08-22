using CustomerManagementSystem.Application.Customer.Interfaces;
using CustomerManagementSystem.Domain.Entitys;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagementSystem.Infrastructure.Persistence.CustomerRepository
{
    public class CustomerQueryRepository : AS.BaseModels.Repository.QueryRepository<Customer>, ICustomerQueryRepository
    {
        public CustomerQueryRepository(DbContext databaseContext) : base(databaseContext)
        {
        }
    }
}
