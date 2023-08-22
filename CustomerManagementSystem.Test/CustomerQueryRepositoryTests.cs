using CustomerManagementSystem.Application.Interfaces;
using CustomerManagementSystem.Domain.Entitys;
using CustomerManagementSystem.Infrastructure.Persistence;
using CustomerManagementSystem.Infrastructure.Persistence.CustomerRepository;
using CustomerManagementSystem.Infrastructure.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagementSystem.Test
{
    public class CustomerQueryRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<DataBaseContext> _options;
        private readonly IQueryUnitOfWork _queryUnitOfWork;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerQueryRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DataBaseContext>()
                .UseInMemoryDatabase("TestConnectionString")
                .Options;

            var dbContext = new DataBaseContext(_options);
            _unitOfWork = new UnitOfWork(new CustomerRepository(dbContext));
            _queryUnitOfWork = new QueryUnitOfWork(new CustomerQueryRepository(dbContext));
        }

        public void Dispose()
        {
            using (var context = new DataBaseContext(_options))
            {
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task GetAsync_AddsCustomerToDatabaseAndGetAsync()
        {
            // Arrange
            var customerToAdd = new Customer
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "1234567890",
                Email = "john.doe@example.com",
                BankAccountNumber = "1234567890"
            };

            // Act
            await _unitOfWork.CustomerRepository.InsertAsync(customerToAdd);

            // Assert
            var retrievedCustomer = await _queryUnitOfWork.CustomerQueryRepository.GetAsync(customerToAdd);
            Assert.NotNull(retrievedCustomer);
            Assert.Equal("John", retrievedCustomer.FirstName);
            Assert.Equal("Doe", retrievedCustomer.LastName);
            // Add more assertions for other properties
        }

        [Fact]
        public async Task GetByIdAsync_RetrievesCustomerById()
        {
            // Arrange
            var customerToAdd = new Customer
            {
                FirstName = "Jane",
                LastName = "Smith",
                DateOfBirth = new DateTime(1985, 5, 5),
                PhoneNumber = "9876543210",
                Email = "jane.smith@example.com",
                BankAccountNumber = "0987654321"
            };

            // Add the customer to the database
            await _unitOfWork.CustomerRepository.InsertAsync(customerToAdd);

            // Act
            var retrievedCustomer = await _queryUnitOfWork.CustomerQueryRepository.GetByIdAsync(customerToAdd.ID);

            // Assert
            Assert.NotNull(retrievedCustomer);
            Assert.Equal("Jane", retrievedCustomer.FirstName);
            Assert.Equal("Smith", retrievedCustomer.LastName);
            // Add more assertions for other properties
        }

        [Fact]
        public async Task GetAllAsync_RetrievesAllCustomers()
        {
            // Arrange
            var customersToAdd = new List<Customer>
        {
            new Customer
            {
                FirstName = "Alice",
                LastName = "Johnson",
                DateOfBirth = new DateTime(1980, 3, 15),
                PhoneNumber = "1111111111",
                Email = "alice@example.com",
                BankAccountNumber = "1111111111"
            },
            new Customer
            {
                FirstName = "Bob",
                LastName = "Williams",
                DateOfBirth = new DateTime(1975, 7, 10),
                PhoneNumber = "2222222222",
                Email = "bob@example.com",
                BankAccountNumber = "2222222222"
            }
        };

            // Add customers to the database
            foreach (var customer in customersToAdd)
            {
                await _unitOfWork.CustomerRepository.InsertAsync(customer);
            }

            // Act
            var retrievedCustomers = await _queryUnitOfWork.CustomerQueryRepository.GetAllAsync();

            // Assert
            Assert.NotNull(retrievedCustomers);
            Assert.Equal(2, retrievedCustomers.Count); // Assuming you added two customers
                                                       // Add more assertions for each customer's properties
        }
    }

}