using CustomerManagementSystem.Application.Customer.Command;
using CustomerManagementSystem.Application.Customer.Dtos;
using CustomerManagementSystem.Application.Interfaces;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CustomerManagementSystem.Application.Customer.CommandHandler
{
    public class CreateCustomerCommandHandler : BaseHandler<CreateCustomerCommand>, IRequestHandler<CreateCustomerCommand, Result<bool>>
    {
        public CreateCustomerCommandHandler(ILogger<CreateCustomerCommand> logger,
                                            IHttpContextAccessor context,
                                            IUnitOfWork unitOfWork,
                                            IQueryUnitOfWork queryUnitOfWork) : base(logger, context, unitOfWork, queryUnitOfWork)
        {
        }

        public async Task<Result<bool>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var result = new Result<bool>();

            try
            {
                await request.CustomerDto.Validate();
                var customer = MapHelper.DynamicMap<CustomerDto, Domain.Entitys.Customer>(request.CustomerDto);

                var res = await _unitOfWork.CustomerRepository.InsertAsync(customer);
                if (!res) throw new Exception("Operation Failed!");

                result.WithSuccess("Done!");
                result.WithValue(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a customer.");
                result.WithError(ex.Message);
            }

            return result; // Return the newly created customer's ID
        }
    }

}
