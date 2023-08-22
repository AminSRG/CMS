using CustomerManagementSystem.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CustomerManagementSystem.Application.Customer
{
    public class BaseCommandHandler<TObject> : object where TObject : class
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly IQueryUnitOfWork _queryUnitOfWork;
        public readonly ILogger<TObject> _logger;
        public readonly IHttpContextAccessor _context;

        public BaseCommandHandler(ILogger<TObject> logger, IHttpContextAccessor context, IUnitOfWork unitOfWork,
            IQueryUnitOfWork queryUnitOfWork)
        {
            _logger = logger;
            _context = context;
            _unitOfWork = unitOfWork;
            _queryUnitOfWork = queryUnitOfWork;
        }
    }
}
