using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CustomerManagementSystem.Application.Customer.Command;
using CustomerManagementSystem.Application.Customer.CommandHandler;
using CustomerManagementSystem.Application.Customer.Dtos;
using CustomerManagementSystem.Application.Customer.Query;
using CustomerManagementSystem.Application.Customer.QueryHandler;
using CustomerManagementSystem.Application.Interfaces;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CustomerManagementSystem.Test
{
    public class CustomerHandlersTests
    {
        // CreateCustomerCommandHandler Test
        [Fact]
        public async Task CreateCustomerCommandHandler_Should_CreateCustomer_Successfully()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CreateCustomerCommand>>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockQueryUnitOfWork = new Mock<IQueryUnitOfWork>();

            var handler = new CreateCustomerCommandHandler(
                mockLogger.Object,
                mockHttpContextAccessor.Object,
                mockUnitOfWork.Object,
                mockQueryUnitOfWork.Object
            );

            var request = new CreateCustomerCommand
            {
                CustomerDto = new CustomerDto
                {
                    // Provide customer properties
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1990, 1, 15),
                    PhoneNumber = "1234567890",
                    Email = "john.doe@example.com",
                    BankAccountNumber = "1234-5678-9012-3456"
                }
            };

            mockUnitOfWork.Setup(u => u.CustomerRepository.InsertAsync(It.IsAny<Domain.Entitys.Customer>()))
                .ReturnsAsync(true);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
        }

        // UpdateCustomerCommandHandler Test
        [Fact]
        public async Task UpdateCustomerCommandHandler_Should_UpdateCustomer_Successfully()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<UpdateCustomerCommand>>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockQueryUnitOfWork = new Mock<IQueryUnitOfWork>();

            var handler = new UpdateCustomerCommandHandler(
                mockLogger.Object,
                mockHttpContextAccessor.Object,
                mockUnitOfWork.Object,
                mockQueryUnitOfWork.Object
            );

            // Create a new customer to insert
            var newCustomer = new Domain.Entitys.Customer
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 15),
                PhoneNumber = "1234567890",
                Email = "john.doe@example.com",
                BankAccountNumber = "1234-5678-9012-3456"
            };

            // Mock the customer repository's InsertAsync method to return the inserted customer
            mockUnitOfWork.Setup(u => u.CustomerRepository.InsertAsync(It.IsAny<Domain.Entitys.Customer>())).ReturnsAsync(true);

            // The request to update the customer
            var request = new UpdateCustomerCommand
            {
                CustomerDto = new CustomerDto
                {
                    // Provide updated customer properties
                    FirstName = "UpdatedFirstName",
                    LastName = "UpdatedLastName",
                    DateOfBirth = new DateTime(1985, 5, 20),
                    PhoneNumber = "9876543210",
                    Email = "updated.email@example.com",
                    BankAccountNumber = "5678-1234-9012-3456"
                }
            };

            // Mock the query repository's SearchCustomer method to return the inserted customer
            mockQueryUnitOfWork.Setup(q => q.CustomerQueryRepository.SearchCustomer(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>()
            )).ReturnsAsync(newCustomer);

            // Mock the customer repository's UpdateAsync method to return true
            mockUnitOfWork.Setup(u => u.CustomerRepository.UpdateAsync(It.IsAny<Domain.Entitys.Customer>()))
                .ReturnsAsync(true);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
        }

        // DeleteCustomerCommandHandler Test
        [Fact]
        public async Task DeleteCustomerCommandHandler_Should_DeleteCustomer_Successfully()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<DeleteCustomerCommand>>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockQueryUnitOfWork = new Mock<IQueryUnitOfWork>();

            var handler = new DeleteCustomerCommandHandler(
                mockLogger.Object,
                mockHttpContextAccessor.Object,
                mockUnitOfWork.Object,
                mockQueryUnitOfWork.Object
            );

            // Create a new customer to delete
            var customerToDelete = new Domain.Entitys.Customer
            {
                FirstName = "ToDelete",
                LastName = "Customer",
                DateOfBirth = new DateTime(1980, 3, 10),
                PhoneNumber = "9876543210",
                Email = "delete.customer@example.com",
                BankAccountNumber = "9999-8888-7777-6666"
            };

            // Mock the query repository's SearchCustomer method to return the customer to delete
            mockQueryUnitOfWork.Setup(q => q.CustomerQueryRepository.SearchCustomer(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>()
            )).ReturnsAsync(customerToDelete);

            // Mock the customer repository's DeleteAsync method to return true
            mockUnitOfWork.Setup(u => u.CustomerRepository.DeleteAsync(It.IsAny<Domain.Entitys.Customer>()))
                .ReturnsAsync(true);

            // The request to delete the customer
            var request = new DeleteCustomerCommand
            {
                CustomerDto = new CustomerDto
                {
                    FirstName = "ToDelete",
                    LastName = "Customer",
                    DateOfBirth = new DateTime(1980, 3, 10),
                }
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
        }

        // GetCustomerByIdQueryHandler Test
        [Fact]
        public async Task GetCustomerByIdQueryHandler_Should_ReturnCustomerById_Successfully()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<GetCustomerByIdQuery>>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockQueryUnitOfWork = new Mock<IQueryUnitOfWork>();

            var handler = new GetCustomerByIdQueryHandler(
                mockLogger.Object,
                mockHttpContextAccessor.Object,
                mockUnitOfWork.Object,
                mockQueryUnitOfWork.Object
            );

            // Create a new customer to retrieve by ID
            var customerToRetrieve = new Domain.Entitys.Customer
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 15),
                PhoneNumber = "1234567890",
                Email = "john.doe@example.com",
                BankAccountNumber = "1234-5678-9012-3456",
                ID = "1" // Assuming "1" is the ID of the customer to retrieve
            };

            // Mock the query repository's GetByIdAsync method to return the customer to retrieve
            mockQueryUnitOfWork.Setup(q => q.CustomerQueryRepository.GetByIdAsync(
                It.IsAny<string>()
            )).ReturnsAsync(customerToRetrieve);

            // The request to get the customer by ID
            var request = new GetCustomerByIdQuery
            {
                CustomerId = "1" // Assuming "1" is the ID of the customer to retrieve
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
        }

        // GetAllCustomersQueryHandler Test
        [Fact]
        public async Task GetAllCustomersQueryHandler_Should_ReturnListOfCustomerDtos_Successfully()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<GetAllCustomersQuery>>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockQueryUnitOfWork = new Mock<IQueryUnitOfWork>();

            var handler = new GetAllCustomersQueryHandler(
                mockLogger.Object,
                mockHttpContextAccessor.Object,
                mockUnitOfWork.Object,
                mockQueryUnitOfWork.Object
            );

            // Create a list of customers to return
            var customersToReturn = new List<Domain.Entitys.Customer>
        {
            new Domain.Entitys.Customer
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 15),
                PhoneNumber = "1234567890",
                Email = "john.doe@example.com",
                BankAccountNumber = "1234-5678-9012-3456"
            },
            new Domain.Entitys.Customer
            {
                FirstName = "Jane",
                LastName = "Smith",
                DateOfBirth = new DateTime(1985, 5, 20),
                PhoneNumber = "9876543210",
                Email = "jane.smith@example.com",
                BankAccountNumber = "5678-1234-9012-3456"
            }
        };

            // Mock the query repository's GetAllAsync method to return the list of customers
            mockQueryUnitOfWork.Setup(q => q.CustomerQueryRepository.GetAllAsync())
                .ReturnsAsync(customersToReturn);

            // The request to get all customers
            var request = new GetAllCustomersQuery();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
        }

        [Fact]
        public async Task CreateCustomerCommandHandler_Should_ThrowValidationException_When_CustomerDataIsInvalid()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CreateCustomerCommand>>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockQueryUnitOfWork = new Mock<IQueryUnitOfWork>();

            var handler = new CreateCustomerCommandHandler(
                mockLogger.Object,
                mockHttpContextAccessor.Object,
                mockUnitOfWork.Object,
                mockQueryUnitOfWork.Object
            );

            var request = new CreateCustomerCommand
            {
                CustomerDto = new CustomerDto
                {
                    // Provide invalid customer data, e.g., missing required properties
                    // FirstName = "", // Missing required field
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1990, 1, 15),
                    PhoneNumber = "1234567890",
                    Email = "invalid-email", // Invalid email format
                    BankAccountNumber = "1234-5678-9012-3456"
                }
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(request, CancellationToken.None));
        }

    }
}