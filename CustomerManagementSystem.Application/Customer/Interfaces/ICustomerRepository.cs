using CustomerManagementSystem.Domain.Entitys;

namespace CustomerManagementSystem.Application.Customer.Interfaces
{
    public interface ICustomerRepository : AS.BaseModels.IRepository.IRepository<Domain.Entitys.Customer>
    {
    }
}
