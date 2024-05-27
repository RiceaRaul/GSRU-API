namespace GSRU_Common.Models.Tasks
{
    public class CreateTaskRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? ParentId { get; set; }
        public int Reporter { get; set; }
        public decimal EstimateTime { get; set; }
        public int StoryPoints { get; set; }
        public int TaskType { get; set; }   
        public int TaskStatus { get; set; }
        public int Priority { get; set; }
        public int TeamId { get; set; }
        public int SprintId { get; set; }
    }
}
