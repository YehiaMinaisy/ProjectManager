using System.ComponentModel.DataAnnotations;

namespace ProjectManagerWebAPI.Dtos.Task
{
    public class AddTaskDto
    {

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public int ProjectId { get; set; }

        public int? ParentId { get; set; }

        public byte[]? AttachmentData { get; set; }

        public string? AttachmentName { get; set; }

        public string? AttachmentType { get; set; }


    }
}
