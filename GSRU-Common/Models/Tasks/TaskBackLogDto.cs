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

        public IEnumerable<TaskAttachment> TaskAttachments { get; set; } = Enumerable.Empty<TaskAttachment>();
        public IEnumerable<TaskComment> TaskComments { get; set; } = Enumerable.Empty<TaskComment>();
        public IEnumerable<TaskAssignee> TaskAssignees { get; set; } = Enumerable.Empty<TaskAssignee>();
    }

    public class TaskDetails
    {
        public IEnumerable<TaskBackLogDto> Children { get; set; } = Enumerable.Empty<TaskBackLogDto>();

        public IEnumerable<TaskAttachment> TaskAttachments { get; set; } = Enumerable.Empty<TaskAttachment>();
        public IEnumerable<TaskComment> TaskComments { get; set; } = Enumerable.Empty<TaskComment>();
        public IEnumerable<TaskAssignee> TaskAssignees { get; set; } = Enumerable.Empty<TaskAssignee>();
    }

    public class TaskAttachment
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
    }

    public class TaskAttachmentDownloadResponse
    {
        public byte[] Bytes { get; set; } = Array.Empty<byte>();
        public string FileName { get; set; } = string.Empty;
    }

    public class TaskComment
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
    }

    public class TaskAssignee
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
    }

    public class TaskCommentsRequest
    {
        public int TaskId { get; set; }
        public int EmployeeId { get; set; }
        public string Comment { get; set; } = string.Empty;
    }

    public class TaskWorkLogRequest
    {
        public int TaskId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
