#nullable disable
namespace DataAccess.Enities
{
    public class Employer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Department { get; set; }
        public string PasswordHash { get; set; } // Store the hashed password
        public string PasswordSalt { get; set; } // Store the password salt for hashing
        public bool IsDeleted { get; set; }
    }
}
