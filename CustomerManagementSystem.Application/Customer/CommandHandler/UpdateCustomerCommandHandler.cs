using CustomerManagementSystem.Application.Customer.Command;
using CustomerManagementSystem.Application.Customer.Dtos;
using CustomerManagementSystem.Application.Interfaces;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CustomerManagementSystem.Application.Customer.CommandHandler
{
    public class UpdateCustomerCommandHandler : BaseHandler<UpdateCustomerCommand>, IRequestHandler<UpdateCustomerCommand, Result<bool>>
    {
        public UpdateCustomerCommandHandler(ILogger<UpdateCustomerCommand> logger,
                                            IHttpContextAccessor context,
                                            IUnitOfWork unitOfWork,
                                            IQueryUnitOfWork queryUnitOfWork) : base(logger, context, unitOfWork, queryUnitOfWork)
        {
        }

        public async Task<Result<bool>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var result = new Result<bool>();

            try
            {
                await request.CustomerDto.Validate();

                var existingCustomer = await _queryUnitOfWork.CustomerQueryRepository.SearchCustomer(firstName: request.CustomerDto.FirstName,
                                                                                                     lastName: request.CustomerDto.LastName,
                                                                                                     dateOfBirth: request.CustomerDto.DateOfBirth);

                if (existingCustomer == null)
                {
                    result.WithError("Customer not found.");
                    return result;
                }

                // Update customer properties as needed
                var updatedCustomer = MapHelper.DynamicMap<CustomerDto, Domain.Entitys.Customer>(request.CustomerDto);

                // Change ID!
                updatedCustomer.ID = existingCustomer.ID;

                var res = await _unitOfWork.CustomerRepository.UpdateAsync(updatedCustomer);
                if (!res) throw new Exception("Update operation failed!");

                result.WithSuccess("Customer updated successfully.");
                result.WithValue(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating a customer.");
                result.WithError(ex.Message);
                result.WithValue(false);
            }

            return result;
        }
    }
}
