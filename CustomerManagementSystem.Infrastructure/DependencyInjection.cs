using CustomerManagementSystem.Application.Customer.Interfaces;
using CustomerManagementSystem.Application.Interfaces;
using CustomerManagementSystem.Infrastructure.Persistence;
using CustomerManagementSystem.Infrastructure.Persistence.CustomerRepository;
using CustomerManagementSystem.Infrastructure.Persistence.Event;
using CustomerManagementSystem.Infrastructure.Persistence.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CustomerManagementSystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<DataBaseContext>(options => options.UseSqlite("Data Source=CustomerDatabase.db"));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerQueryRepository, CustomerQueryRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IQueryUnitOfWork, QueryUnitOfWork>();


            services.AddScoped<IEventBroker, InMemoryEventBroker>();
            services.AddScoped<IEventStore, InMemoryEventStore>();


            return services;
        }
    }
}
