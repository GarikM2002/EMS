﻿#nullable disable
namespace EMS.API.Models
{
    public class RegisterViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Department { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}