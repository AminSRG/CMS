using FluentValidation;
using PhoneNumbers;

namespace CustomerManagementSystem.Application.Customer.Dtos
{
    public class CustomerDtoValidator : AbstractValidator<CustomerDto>
    {
        public CustomerDtoValidator()
        {
            RuleFor(customer => customer.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");

            RuleFor(customer => customer.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");

            RuleFor(customer => customer.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required")
                .Must(BeAValidDate).WithMessage("Invalid date of birth");

            RuleFor(customer => customer.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .Must(BeValidMobilePhoneNumber).WithMessage("Invalid mobile phone number");

            RuleFor(customer => customer.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email address");

            RuleFor(customer => customer.BankAccountNumber)
                .NotEmpty().WithMessage("Bank account number is required")
                .Must(BeValidBankAccountNumber).WithMessage("Invalid bank account number");
        }

        private bool BeAValidDate(DateTime date)
        {
            // Example: Date should not be in the future
            return date <= DateTime.Now;
        }


        private bool BeValidMobilePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return false; // Empty or null phone number is not valid
            }

            // Extract the first three characters from the phone number
            string countryCode = phoneNumber.Substring(0, 3);

            PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();
            try
            {
                PhoneNumber number = phoneNumberUtil.Parse(phoneNumber, countryCode);
                return phoneNumberUtil.IsValidNumber(number);
            }
            catch (Exception)
            {
                return false;
            }
        }


        private bool BeValidBankAccountNumber(string bankAccountNumber)
        {
            // Check if the email has the format "****-****-****-****"
            if (string.IsNullOrWhiteSpace(bankAccountNumber) || bankAccountNumber.Length != 19)
            {
                return false;
            }

            string[] sections = bankAccountNumber.Split('-');

            // Check if there are four sections, each containing four characters
            if (sections.Length != 4 || sections.Any(section => section.Length != 4))
            {
                return false;
            }

            // Check if each section consists of alphanumeric characters
            foreach (string section in sections)
            {
                if (!section.All(char.IsLetterOrDigit))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
