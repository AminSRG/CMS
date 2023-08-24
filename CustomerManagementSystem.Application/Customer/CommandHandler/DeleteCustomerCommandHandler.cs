using CustomerManagementSystem.Application.Customer.Command;
using CustomerManagementSystem.Application.Interfaces;
using CustomerManagementSystem.Domain.Event;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CustomerManagementSystem.Application.Customer.CommandHandler
{
    public class DeleteCustomerCommandHandler : BaseCommandHandler<DeleteCustomerCommand>, IRequestHandler<DeleteCustomerCommand, Result<bool>>
    {
        public DeleteCustomerCommandHandler(ILogger<DeleteCustomerCommand> logger,
                                            IHttpContextAccessor context,
                                            IUnitOfWork unitOfWork,
                                            IQueryUnitOfWork queryUnitOfWork,
                                            IEventBroker eventBroker,
                                            IEventStore eventStore) : base(logger, context, unitOfWork, queryUnitOfWork, eventBroker, eventStore)
        {
        }

        public async Task<Result<bool>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var result = new Result<bool>();

            try
            {
                var existingCustomer = await _queryUnitOfWork.CustomerQueryRepository.GetByIdAsync(request.CustomerId);

                if (existingCustomer == null)
                {
                    result.WithError("Customer not found.");
                    return result;
                }

                // Publish a CustomerDeletedEvent
                var deletedEvent = new CustomerDeletedEvent
                {
                    CustomerId = existingCustomer.ID
                };
                _eventBroker.Publish(deletedEvent);

                // Store the event in the event store
                await _eventStore.AppendEventsAsync(existingCustomer.ID, new List<CustomerEvent> { deletedEvent });

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
