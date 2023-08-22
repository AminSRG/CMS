using CustomerManagementSystem.Application.Customer.Dtos;
using CustomerManagementSystem.Application.Customer.Query;
using CustomerManagementSystem.Application.Interfaces;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CustomerManagementSystem.Application.Customer.QueryHandler
{
    public class GetCustomerByIdQueryHandler : BaseHandler<GetCustomerByIdQuery>, IRequestHandler<GetCustomerByIdQuery, Result<CustomerDto>>
    {
        public GetCustomerByIdQueryHandler(ILogger<GetCustomerByIdQuery> logger,
                                           IHttpContextAccessor context,
                                           IUnitOfWork unitOfWork,
                                           IQueryUnitOfWork queryUnitOfWork) : base(logger, context, unitOfWork, queryUnitOfWork)
        {
        }

        public async Task<Result<CustomerDto>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var result = new Result<CustomerDto>();

            try
            {
                var customer = await _queryUnitOfWork.CustomerQueryRepository.GetByIdAsync(request.CustomerId);

                if (customer == null)
                {
                    result.WithError("Customer not found.");
                    return result;
                }

                var customerDto = MapHelper.DynamicMap<Domain.Entitys.Customer, CustomerDto>(customer);

                result.WithSuccess("Customer retrieved successfully.");
                result.WithValue(customerDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving a customer.");
                result.WithError(ex.Message);
            }

            return result;
        }
    }

}
