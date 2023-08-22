using CustomerManagementSystem.Application.Customer.Dtos;
using CustomerManagementSystem.Application.Customer.Query;
using CustomerManagementSystem.Application.Interfaces;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CustomerManagementSystem.Application.Customer.QueryHandler
{
    public class GetAllCustomersQueryHandler : BaseHandler<GetAllCustomersQuery>, IRequestHandler<GetAllCustomersQuery, Result<List<CustomerDto>>>
    {
        public GetAllCustomersQueryHandler(ILogger<GetAllCustomersQuery> logger,
                                           IHttpContextAccessor context,
                                           IUnitOfWork unitOfWork,
                                           IQueryUnitOfWork queryUnitOfWork) : base(logger, context, unitOfWork, queryUnitOfWork)
        {
        }

        public async Task<Result<List<CustomerDto>>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var result = new Result<List<CustomerDto>>();

            try
            {
                var customers = await _queryUnitOfWork.CustomerQueryRepository.GetAllAsync();

                var customerDtos = MapHelper.DynamicMapList<Domain.Entitys.Customer, CustomerDto>(customers);

                result.WithSuccess("Customers retrieved successfully.");
                result.WithValue(customerDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving customers.");
                result.WithError(ex.Message);
            }

            return result;
        }
    }

}
