using CustomerManagementSystem.Domain.Entitys;

namespace CustomerManagementSystem.Application.Customer.Interfaces
{
    public interface ICustomerQueryRepository : AS.BaseModels.IRepository.IQueryRepository<Domain.Entitys.Customer>
    {
    }
}
