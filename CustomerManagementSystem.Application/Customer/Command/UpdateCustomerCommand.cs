using CustomerManagementSystem.Application.Customer.Dtos;
using FluentResults;
using MediatR;

namespace CustomerManagementSystem.Application.Customer.Command
{
    public class UpdateCustomerCommand : IRequest<Result<bool>>
    {
        public string CustomerId { get; set; }
        public CustomerDto CustomerDto { get; set; }
    }


}
