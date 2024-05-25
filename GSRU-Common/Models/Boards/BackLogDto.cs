using GSRU_API.Common.Models;

namespace GSRU_Common.Models.Boards
{
    public class BackLogDto : GenericError<string>
    {
        public IEnumerable<SprintDto> Sprints { get; set; } = new List<SprintDto>();
    }
}
