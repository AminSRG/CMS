using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace CustomerManagementSystem.Application.Customer.Dtos
{
    public class CustomerDto
    {
        public CustomerDto()
        {

        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string BankAccountNumber { get; set; }

        public async Task Validate()
        {
            var validator = new CustomerDtoValidator();
            var validationResult = await validator.ValidateAsync(this);

            if (!validationResult.IsValid) throw new FluentValidation.ValidationException(validationResult.Errors);
        }
    }
}
