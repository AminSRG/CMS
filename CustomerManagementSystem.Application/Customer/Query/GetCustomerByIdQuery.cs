using CustomerManagementSystem.Application.Customer.Dtos;
using CustomerManagementSystem.Domain.Entitys;
using FluentResults;
using MediatR;

namespace CustomerManagementSystem.Application.Customer.Query
{
    public class GetCustomerByIdQuery : IRequest<Result<CustomerDto>>
    {
        public string CustomerId { get; set; }
    }
}
