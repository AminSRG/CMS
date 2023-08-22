using CustomerManagementSystem.Application.Customer.Dtos;
using FluentResults;
using MediatR;

namespace CustomerManagementSystem.Application.Customer.Command
{
    public class CreateCustomerCommand : IRequest<Result<bool>>
    {
        public CustomerDto CustomerDto { get; set; }
    }

}
