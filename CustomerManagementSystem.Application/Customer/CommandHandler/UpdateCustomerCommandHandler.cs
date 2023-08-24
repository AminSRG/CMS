using CustomerManagementSystem.Application.Customer.Command;
using CustomerManagementSystem.Application.Customer.Dtos;
using CustomerManagementSystem.Application.Interfaces;
using CustomerManagementSystem.Domain.Event;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CustomerManagementSystem.Application.Customer.CommandHandler
{
    public class UpdateCustomerCommandHandler : BaseCommandHandler<UpdateCustomerCommand>, IRequestHandler<UpdateCustomerCommand, Result<bool>>
    {
        public UpdateCustomerCommandHandler(ILogger<UpdateCustomerCommand> logger,
                                            IHttpContextAccessor context,
                                            IUnitOfWork unitOfWork,
                                            IQueryUnitOfWork queryUnitOfWork,
                                            IEventBroker eventBroker,
                                            IEventStore eventStore) : base(logger, context, unitOfWork, queryUnitOfWork, eventBroker, eventStore)
        {
        }

        public async Task<Result<bool>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var result = new Result<bool>();

            try
            {
                var validationResult = await request.CustomerDto.Validate();

                if (!validationResult.IsValid) return result.AddValidationErrors<bool>(validationResult);

                var existingCustomer = await _queryUnitOfWork.CustomerQueryRepository.GetByIdAsync(request.CustomerId);

                if (existingCustomer == null)
                {
                    result.WithError("Customer not found.");
                    return result;
                }

                // Create a CustomerUpdatedEvent
                var updatedEvent = new CustomerUpdatedEvent
                {
                    CustomerId = existingCustomer.ID,
                    FirstName = request.CustomerDto.FirstName,
                    LastName = request.CustomerDto.LastName,
                    DateOfBirth = request.CustomerDto.DateOfBirth,
                    PhoneNumber = request.CustomerDto.PhoneNumber,
                    Email = request.CustomerDto.Email,
                    BankAccountNumber = request.CustomerDto.BankAccountNumber
                };

                // Publish the CustomerUpdatedEvent
                _eventBroker.Publish(updatedEvent);

                // Store the event in the event store
                await _eventStore.AppendEventsAsync(existingCustomer.ID, new List<CustomerEvent> { updatedEvent });

                // Update customer properties as needed
                existingCustomer.FirstName = request.CustomerDto.FirstName;
                existingCustomer.LastName = request.CustomerDto.LastName;   
                existingCustomer.DateOfBirth = request.CustomerDto.DateOfBirth;
                existingCustomer.PhoneNumber = request.CustomerDto.PhoneNumber;
                existingCustomer.Email = request.CustomerDto.Email;
                existingCustomer.BankAccountNumber = request.CustomerDto.BankAccountNumber;

                var res = await _unitOfWork.CustomerRepository.UpdateAsync(existingCustomer);
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
