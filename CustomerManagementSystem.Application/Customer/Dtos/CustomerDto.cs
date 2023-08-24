using FluentValidation;
using FluentValidation.Results;

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

        public async Task<ValidationResult> Validate()
        {
            var validator = new CustomerDtoValidator();
            return await validator.ValidateAsync(this);
        }
    }
}
