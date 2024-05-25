using GSRU_Common.Models.Boards;

namespace GSRU_Common.Models.Requests.Tasks
{
    public class TaskUpdateSprintAndIndexRequest
    {
        public IEnumerable<TaskUpdateSprintAndIndex> Tasks { get; set; } =  Enumerable.Empty<TaskUpdateSprintAndIndex>();
    }
}
