using CustomerManagementSystem.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CustomerManagementSystem.Application.Customer
{
    public class BaseHandler<TObject> : object where TObject : class
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly IQueryUnitOfWork _queryUnitOfWork;
        public readonly ILogger<TObject> _logger;
        public readonly IHttpContextAccessor _context;

        public BaseHandler(ILogger<TObject> logger, IHttpContextAccessor context, IUnitOfWork unitOfWork,
            IQueryUnitOfWork queryUnitOfWork)
        {
            _logger = logger;
            _context = context;
            _unitOfWork = unitOfWork;
            _queryUnitOfWork = queryUnitOfWork;
        }
    }

    public class BaseCommandHandler<T> : BaseHandler<T> where T : class
    {
        public readonly IEventBroker _eventBroker;
        public readonly IEventStore  _eventStore;

        public BaseCommandHandler(ILogger<T> logger,
                                  IHttpContextAccessor context,
                                  IUnitOfWork unitOfWork,
                                  IQueryUnitOfWork queryUnitOfWork,
                                  IEventBroker eventBroker,
                                  IEventStore eventStore) : base(logger, context, unitOfWork, queryUnitOfWork)
        {
            _eventBroker = eventBroker;
            _eventStore = eventStore;
        }
    }
}
