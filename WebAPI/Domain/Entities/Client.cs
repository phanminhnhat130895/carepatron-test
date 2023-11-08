using Domain.Common;
using System.ComponentModel.DataAnnotations;

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

        [Required]
        [MaxLength(255)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(255)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(500)]
        public string Email { get; set; }

        [Required]
        [MaxLength(25)]
        public string PhoneNumber { get; set; }
    }
}
