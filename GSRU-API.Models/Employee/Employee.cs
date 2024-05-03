using Microsoft.AspNetCore.Identity;

namespace GSRU_API.Models.Employee
{
    public class Employee
    {
        public int Id { get; set; }
        public float Salary { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Password { get; set; } = string.Empty;
        public string CompanyEmail { get; set; } = string.Empty;
        public int TotalLeaves { get; set; }
        public int RemainingLeaves { get; set; }
        public int WorkHours { get; set; }
        public Person Person { get; set; } = new Person();
        public Address Address { get; set; } = new Address();
    }

    public class Person
    {
        [PersonalData]
        public string FirstName { get; set; } = string.Empty;
        [PersonalData]
        public string LastName { get; set; } = string.Empty;

        [ProtectedPersonalData]
        public string CNP { get; set; } = string.Empty;

        [ProtectedPersonalData]
        public string SerieNr { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; }
        
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public class Address
    {
        public string Adress1 { get; set; } = string.Empty;
        public string Adress2 { get; set; } = string.Empty;
    }
}
