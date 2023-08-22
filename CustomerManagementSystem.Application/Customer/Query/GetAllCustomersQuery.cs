using CustomerManagementSystem.Application.Customer.Dtos;
using FluentResults;
using MediatR;

namespace CustomerManagementSystem.Application.Customer.Query
{
    public class GetAllCustomersQuery : IRequest<Result<List<CustomerDto>>>
    {
    }

}
