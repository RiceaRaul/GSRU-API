using System.Diagnostics.CodeAnalysis;

namespace GSRU_Common.Models.Tasks
{
    public class AssignEmployeeToTaskRequest
    {
        [AllowNull]
        public int? EmployeeId { get; set; }
        public int TaskId { get; set; }
    }
}
