using CustomerManagementSystem.Domain.Base;
using CustomerManagementSystem.Domain.Event;

namespace CustomerManagementSystem.Domain.Entitys
{
    public class Customer : BaseModel
    {
        private List<CustomerEvent> _events = new List<CustomerEvent>();

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string BankAccountNumber { get; set; }
        private bool IsDeleted { get; set; }

        public Customer() { }

        public Customer(string id, string firstName, string lastName, DateTime dateOfBirth, string phoneNumber, string email, string bankAccountNumber)
        {
            ApplyChange(new CustomerCreatedEvent
            {
                CustomerId = id,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                PhoneNumber = phoneNumber,
                Email = email,
                BankAccountNumber = bankAccountNumber
            });
        }

        public void Update(string firstName, string lastName, DateTime dateOfBirth, string phoneNumber, string email, string bankAccountNumber)
        {
            ApplyChange(new CustomerUpdatedEvent
            {
                CustomerId = ID,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                PhoneNumber = phoneNumber,
                Email = email,
                BankAccountNumber = bankAccountNumber
            });
        }

        public void Delete()
        {
            if (!IsDeleted)
            {
                ApplyChange(new CustomerDeletedEvent
                {
                    CustomerId = ID
                });
            }
        }

        private void ApplyChange(CustomerEvent @event)
        {
            // Apply the event to change the state of the entity
            // You might also perform validation and raise additional events here
            _events.Add(@event);
            When(@event);
        }

        private void When(CustomerEvent @event)
        {
            switch (@event)
            {
                case CustomerCreatedEvent created:
                    ID = created.CustomerId;
                    FirstName = created.FirstName;
                    LastName = created.LastName;
                    DateOfBirth = created.DateOfBirth;
                    PhoneNumber = created.PhoneNumber;
                    Email = created.Email;
                    BankAccountNumber = created.BankAccountNumber;
                    break;
                case CustomerUpdatedEvent updated:
                    FirstName = updated.FirstName;
                    LastName = updated.LastName;
                    DateOfBirth = updated.DateOfBirth;
                    PhoneNumber = updated.PhoneNumber;
                    Email = updated.Email;
                    BankAccountNumber = updated.BankAccountNumber;
                    break;
                case CustomerDeletedEvent deleted:
                    IsDeleted = true;
                    break;
            }
        }

        public List<CustomerEvent> GetUncommittedEvents()
        {
            return _events;
        }
    }

}
