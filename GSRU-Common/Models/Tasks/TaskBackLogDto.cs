namespace GSRU_Common.Models.Tasks
{
    public class TaskBackLogDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? ParentId { get; set; }
        public string Reporter { get; set; } = string.Empty;
        public int EstimateTime { get; set; }
        public int RemainingTime { get; set; }
        public int StoryPoints { get; set; }
        public string TaskType { get; set; } = string.Empty;
        public string TaskStatus { get; set; } = string.Empty;
        public int Priority { get; set; }
        public int Index { get; set; }
        public int SprintId { get; set; }
        public IEnumerable<TaskBackLogDto> Children { get; set; } = Enumerable.Empty<TaskBackLogDto>();
    }
}
