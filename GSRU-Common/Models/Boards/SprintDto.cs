using GSRU_API.Common.Models;
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

    public class SprintDtoResponse : GenericError<string>
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string SprintGoal { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IEnumerable<TaskBackLogDto> Tasks { get; set; } = new List<TaskBackLogDto>();

        public static implicit operator SprintDtoResponse(SprintDto sprint)
        {
            return new SprintDtoResponse
            {
                Id = sprint.Id,
                Number = sprint.Number,
                SprintGoal = sprint.SprintGoal,
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate,
                Tasks = sprint.Tasks
            };
        }
    }

    public class StartSprintRequest
    {
        public int Id { get; set; }
        public string SprintGoal { get; set; } = string.Empty;
    }

    public class BoardConfigurationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NameShort => Name.ToUpper();
    }

    public class BoardConfigurationDtoResponse : GenericError<string>
    {
        public IEnumerable<BoardConfigurationDto> Configuration { get; set; } = Enumerable.Empty<BoardConfigurationDto>();
    }
}
