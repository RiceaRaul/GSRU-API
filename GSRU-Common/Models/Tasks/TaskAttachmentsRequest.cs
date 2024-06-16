using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GSRU_Common.Models.Tasks
{
    public class TaskAttachmentsRequest
    {
        [FromForm(Name ="taskId")]
        public int TaskId { get; set; }

        [FromForm(Name = "files")]
        public IFormFileCollection Files { get; set; }
    }
}
