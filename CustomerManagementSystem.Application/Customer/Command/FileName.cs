using CustomerManagementSystem.Application.Customer.Dtos;
using MediatR;

namespace CustomerManagementSystem.Application.Customer.Command
{
    public class GetCustomerByIdQuery : IRequest<CustomerDto>
    {
        public int CustomerId { get; set; }
    }

    public class CreateCustomerCommand : IRequest<int>
    {
        public CustomerDto CustomerDto { get; set; }
    }

}
