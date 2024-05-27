using GSRU_Common.Models.Tasks;

namespace GSRU_Common.Models.Boards
{
    public class SprintDto
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string SprintGoal { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IEnumerable<TaskBackLogDto> Tasks { get; set; } = new List<TaskBackLogDto>();
    }

    public class StartSprintRequest
    {
        public int Id { get; set; }
        public string SprintGoal { get; set; } = string.Empty;
    }
}
