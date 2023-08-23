using CustomerManagementSystem.Application.Customer.CommandHandler;
using CustomerManagementSystem.Application.Customer.QueryHandler;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net.NetworkInformation;
using System.Reflection;

namespace CustomerManagementSystem.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                 cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommandHandler).Assembly));
            services.AddMediatR(cfg =>
                 cfg.RegisterServicesFromAssembly(typeof(GetAllCustomersQueryHandler).Assembly));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient<IHttpContextAccessor,HttpContextAccessor>();

            services.AddTransient
                (serviceType: typeof(Microsoft.Extensions.Logging.ILogger<>),
                implementationType: typeof(Microsoft.Extensions.Logging.Logger<>));

            return services;
        }
    }
}
