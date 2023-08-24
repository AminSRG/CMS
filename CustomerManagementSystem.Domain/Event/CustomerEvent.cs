using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagementSystem.Domain.Event
{
    public abstract class CustomerEvent
    {
        public string CustomerId { get; set; }
    }

    public class CustomerCreatedEvent : CustomerEvent
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string BankAccountNumber { get; set; }
    }

    public class CustomerUpdatedEvent : CustomerEvent
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string BankAccountNumber { get; set; }
    }

    public class CustomerDeletedEvent : CustomerEvent { }
}
