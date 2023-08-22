using CustomerManagementSystem.Application.Customer.Interfaces;
using CustomerManagementSystem.Domain.Entitys;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagementSystem.Infrastructure.Persistence.CustomerRepository
{
    public class CustomerRepository : AS.BaseModels.Repository.Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(DbContext databaseContext) : base(databaseContext)
        {
        }
    }
}
