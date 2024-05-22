using GSRU_API.Common.Models;
using GSRU_API.Common.Models.Employee.Dto;
using GSRU_Common.Models.Teams.Dto;

namespace GSRU_Common.Models.Employee.Responses
{
    public class EmployeeData : GenericError<string>
    {
        public int? Id { get; init; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public DateTime BirthDate { get; init; }
        public string Email { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        public DateTime HireDate { get; init; }
        public int TotalLeaves { get; init; }
        public int RemainingLeaves { get; init; }
        public int WorkHours { get; init; }
        public string? Address1 { get; init; } = string.Empty;
        public string Address2 { get; init; } = string.Empty;

        public IEnumerable<TeamDto> Teams { get; set; } = Enumerable.Empty<TeamDto>();

        public static implicit operator EmployeeData(EmployeeDto data)
        {
            return new EmployeeData
            {
                Id = data.Id,
                FirstName = data.FirstName,
                LastName = data.LastName,
                BirthDate = data.BirthDate,
                Email = data.Email,
                PhoneNumber = data.PhoneNumber,
                HireDate = data.HireDate,
                TotalLeaves = data.TotalLeaves,
                RemainingLeaves = data.RemainingLeaves,
                WorkHours = data.WorkHours,
                Address1 = data.Address1,
                Address2 = data.Address2,
                Teams = data.Teams
            };
        }
    }
}
