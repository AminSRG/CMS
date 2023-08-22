using CustomerManagementSystem.Application.Interfaces;
using CustomerManagementSystem.Domain.Entitys;
using CustomerManagementSystem.Infrastructure.Persistence;
using CustomerManagementSystem.Infrastructure.Persistence.CustomerRepository;
using CustomerManagementSystem.Infrastructure.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CustomerManagementSystem.Test
{
    public class CustomerRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<DataBaseContext> _options;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DataBaseContext>()
                .UseInMemoryDatabase("TestConnectionString")
                .Options;

            var dbContext = new DataBaseContext(_options);
            _unitOfWork = new UnitOfWork(new CustomerRepository(dbContext));
        }

        public void Dispose()
        {
            using (var context = new DataBaseContext(_options))
            {
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task AddCustomer_AddsCustomerToDatabaseAsync()
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
            var retrievedCustomer = await _unitOfWork.CustomerRepository.GetByIdAsync(customerToAdd.ID);
            Assert.NotNull(retrievedCustomer);
            Assert.Equal("John", retrievedCustomer.FirstName);
            Assert.Equal("Doe", retrievedCustomer.LastName);
            // Add more assertions for other properties
        }

        [Fact]
        public async Task UpdateCustomer_UpdatesCustomerInformationAsync()
        {
            // Arrange
            var customerToUpdate = new Customer
            {
                FirstName = "Jane",
                LastName = "Smith",
                DateOfBirth = new DateTime(1985, 5, 5),
                PhoneNumber = "9876543210",
                Email = "jane.smith@example.com",
                BankAccountNumber = "0987654321"
            };

            await _unitOfWork.CustomerRepository.InsertAsync(customerToUpdate);


            // Act
            var updatedCustomer = new Customer
            {
                ID = customerToUpdate.ID,
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "1234567890",
                Email = "updated.email@example.com",
                BankAccountNumber = "9876543210"
            };

            await _unitOfWork.CustomerRepository.UpdateAsync(updatedCustomer);

            // Assert
            var retrievedCustomer = await _unitOfWork.CustomerRepository.GetByIdAsync(updatedCustomer.ID);
            Assert.Equal(updatedCustomer.ID, retrievedCustomer.ID);
            Assert.Equal(updatedCustomer.Email, retrievedCustomer.Email);
            Assert.Equal(updatedCustomer.PhoneNumber, retrievedCustomer.PhoneNumber);
            // Add more assertions for other properties
        }

        [Fact]
        public async Task DeleteCustomer_RemovesCustomerFromDatabaseAsync()
        {
            // Arrange
            var customerToDelete = new Customer
            {
                FirstName = "Jane",
                LastName = "Smith",
                DateOfBirth = new DateTime(1985, 5, 5),
                PhoneNumber = "9876543210",
                Email = "jane.smith@example.com",
                BankAccountNumber = "0987654321"
            };

            await _unitOfWork.CustomerRepository.InsertAsync(customerToDelete);

            // Act
            await _unitOfWork.CustomerRepository.DeleteAsync(customerToDelete);

            // Assert
            var retrievedCustomer = await _unitOfWork.CustomerRepository.GetByIdAsync(customerToDelete.ID);
            Assert.Null(retrievedCustomer); // Customer should not exist after deletion
        }

        [Fact]
        public async Task AddCustomer_FailsForNonUniqueCustomerAsync()
        {
            // Arrange
            var customer1 = new Customer
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "1234567890",
                Email = "john.doe@example.com", // Same email as customer1
                BankAccountNumber = "1234567890"
            };

            var customer2 = new Customer
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "1234567890",
                Email = "john.doe@example.com", // Same email as customer1
                BankAccountNumber = "1234567890"
            };

            // Act & Assert
            await _unitOfWork.CustomerRepository.InsertAsync(customer1);

            // Attempt to add a second customer with the same details
            // This should fail due to the uniqueness constraint
            await Assert.ThrowsAsync<DbUpdateException>(async () =>
            {
                await _unitOfWork.CustomerRepository.InsertAsync(customer2);
            });
        }

        [Fact]
        public async Task GetCustomerById_GetInsertedCustomerAsync()
        {
            // Arrange
            var customer = new Customer
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                PhoneNumber = "1234567890",
                Email = "InsertForGetById.doe@example.com",
                BankAccountNumber = "1234567890"
            };

            // Act & Assert
            await _unitOfWork.CustomerRepository.InsertAsync(customer);

            // Attempt to add a second customer with the same details
            // This should fail due to the uniqueness constraint
            var retrivedCustomer = await _unitOfWork.CustomerRepository.GetByIdAsync(customer.ID);
            Assert.NotNull(retrivedCustomer);
        }
    }
}