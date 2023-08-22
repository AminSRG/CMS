using CustomerManagementSystem.Application.Customer.Interfaces;
using CustomerManagementSystem.Domain.Entitys;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagementSystem.Infrastructure.Persistence.CustomerRepository
{
    public class CustomerQueryRepository : AS.BaseModels.Repository.QueryRepository<Customer>, ICustomerQueryRepository
    {
        public CustomerQueryRepository(DataBaseContext databaseContext) : base(databaseContext)
        {
        }


        public async Task<Customer> SearchCustomer(string firstName, string lastName, DateTime dateOfBirth)
        {
            return await DbSet.Where(current => current.FirstName.ToLower() == firstName.ToLower()
            && current.LastName.ToLower() == lastName.ToLower()
            && current.DateOfBirth == dateOfBirth).FirstAsync();
        }
    }
}
