using CustomerManagementSystem.Application.Customer.Command;
using CustomerManagementSystem.Application.Interfaces;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CustomerManagementSystem.Application.Customer.CommandHandler
{
    public class DeleteCustomerCommandHandler : BaseHandler<DeleteCustomerCommand>, IRequestHandler<DeleteCustomerCommand, Result<bool>>
    {
        public DeleteCustomerCommandHandler(ILogger<DeleteCustomerCommand> logger,
                                            IHttpContextAccessor context,
                                            IUnitOfWork unitOfWork,
                                            IQueryUnitOfWork queryUnitOfWork) : base(logger, context, unitOfWork, queryUnitOfWork)
        {
        }

        public async Task<Result<bool>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
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

                var res = await _unitOfWork.CustomerRepository.DeleteAsync(existingCustomer);
                if (!res) throw new Exception("Delete operation failed!");

                result.WithSuccess("Customer deleted successfully.");
                result.WithValue(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting a customer.");
                result.WithError(ex.Message);
                result.WithValue(false);
            }

            return result;
        }
    }

}
