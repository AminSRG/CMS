using CustomerManagementSystem.Domain.Entitys;

namespace CustomerManagementSystem.Application.Customer.Interfaces
{
    public interface ICustomerQueryRepository : AS.BaseModels.IRepository.IQueryRepository<Domain.Entitys.Customer>
    {
        Task<Domain.Entitys.Customer> SearchCustomer(string firstName, string lastName, DateTime dateOfBirth);
    }
}
