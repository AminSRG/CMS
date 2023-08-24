using CustomerManagementSystem.Application.Customer.Command;
using CustomerManagementSystem.Application.Customer.Dtos;
using CustomerManagementSystem.Application.Interfaces;
using CustomerManagementSystem.Domain.Event;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CustomerManagementSystem.Application.Customer.CommandHandler
{
    public class CreateCustomerCommandHandler : BaseCommandHandler<CreateCustomerCommand>, IRequestHandler<CreateCustomerCommand, Result<string>>
    {
        public CreateCustomerCommandHandler(ILogger<CreateCustomerCommand> logger,
                                            IHttpContextAccessor context,
                                            IUnitOfWork unitOfWork,
                                            IQueryUnitOfWork queryUnitOfWork,
                                            IEventBroker eventBroker,
                                            IEventStore eventStore) : base(logger, context, unitOfWork, queryUnitOfWork, eventBroker, eventStore)
        {
        }

        public async Task<Result<string>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var result = new Result<string>();

            try
            {
                var validationResult = await request.CustomerDto.Validate();

                if (!validationResult.IsValid) return result.AddValidationErrors<string>(validationResult);

                // Create a new customer
                var customer = MapHelper.DynamicMap<CustomerDto, Domain.Entitys.Customer>(request.CustomerDto);

                // Publish a CustomerCreatedEvent
                var createdEvent = new CustomerCreatedEvent
                {
                    CustomerId = customer.ID,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    DateOfBirth = customer.DateOfBirth,
                    PhoneNumber = customer.PhoneNumber,
                    Email = customer.Email,
                    BankAccountNumber = customer.BankAccountNumber
                };
                _eventBroker.Publish(createdEvent);

                // Store the event in the event store
                await _eventStore.AppendEventsAsync(customer.ID, new List<CustomerEvent> { createdEvent });

                // Persist the customer entity in the database
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
