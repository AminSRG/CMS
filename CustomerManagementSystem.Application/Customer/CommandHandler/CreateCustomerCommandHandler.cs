using CustomerManagementSystem.Application.Customer.Command;
using CustomerManagementSystem.Application.Customer.Dtos;
using CustomerManagementSystem.Application.Interfaces;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CustomerManagementSystem.Application.Customer.CommandHandler
{
    public class CreateCustomerCommandHandler : BaseHandler<CreateCustomerCommand>, IRequestHandler<CreateCustomerCommand, Result<string>>
    {
        public CreateCustomerCommandHandler(ILogger<CreateCustomerCommand> logger,
                                            IHttpContextAccessor context,
                                            IUnitOfWork unitOfWork,
                                            IQueryUnitOfWork queryUnitOfWork) : base(logger, context, unitOfWork, queryUnitOfWork)
        {
        }

        public async Task<Result<string>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var result = new Result<string>();

            try
            {
                var validationResult = await request.CustomerDto.Validate();
                
                if (!validationResult.IsValid) return result.AddValidationErrors<string>(validationResult);

                var customer = MapHelper.DynamicMap<CustomerDto, Domain.Entitys.Customer>(request.CustomerDto);

                var res = await _unitOfWork.CustomerRepository.InsertAsync(customer);
                if (!res) throw new Exception("Operation Failed!");

                result.WithSuccess("Done!");
                result.WithValue(customer.ID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a customer.");
                result.WithError(ex.Message);
            }

            return result;
        }


    }

}
