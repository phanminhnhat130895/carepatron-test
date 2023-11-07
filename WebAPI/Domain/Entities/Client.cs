using Domain.Common;

namespace Domain.Entities
{
    public class Client : Entity
    {
        public Client(Guid id) : base(id) { }

        public Client(Guid id, string firstName, string lastName, string email, string phoneNumber) : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}
