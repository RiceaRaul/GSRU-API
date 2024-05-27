using GSRU_API.Common.Models;

namespace GSRU_Common.Models.Tasks
{
    public class TaskTypeStatusDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class TaskTypeStatusResponse : GenericError<string>
    {
        public IEnumerable<TaskTypeStatusDto> Result { get; set; } = Enumerable.Empty<TaskTypeStatusDto>();
    }
}
