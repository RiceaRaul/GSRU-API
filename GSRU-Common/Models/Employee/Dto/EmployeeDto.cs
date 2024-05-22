using GSRU_Common.Models.Teams.Dto;

namespace GSRU_API.Common.Models.Employee.Dto
{    
    public class EmployeeDto : GenericError<string>
    {
        public int? Id { get; init; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Cnp { get; init; } = string.Empty;
        public string SerieNr { get; init; } = string.Empty;
        public DateTime BirthDate { get; init; }
        public string Email { get; init; } = string.Empty;
        public decimal? Salary { get; init; }
        public string PhoneNumber { get; init; } = string.Empty;
        public DateTime HireDate { get; init; }
        public string Password { get; init; } = string.Empty;
        public string CompanyEmail { get; init; } = string.Empty;
        public int TotalLeaves { get; init; }
        public int RemainingLeaves { get; init; }
        public int WorkHours { get; init; }
        public string? Address1 { get; init; } = string.Empty;
        public string Address2 { get; init; } = string.Empty;

        public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
        public IEnumerable<TeamDto> Teams { get; set; } = Enumerable.Empty<TeamDto>();
    }
}
