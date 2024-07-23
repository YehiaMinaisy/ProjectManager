using ProjectManagerWebAPI.Dtos.Project;
using ProjectManagerWebAPI.Dtos.User;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagerWebAPI.Dtos.Task
{
    public class TaskDto
    {
        public int Id { get; set; }

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

        public virtual ICollection<TaskDto> subTasks { get; set; } = new List<TaskDto>();

        public virtual TaskDto? Parent { get; set; }

        public virtual ProjectDto? Project { get; set; } = null!;
        public virtual ICollection<UserDto> Users { get; set; } = new List<UserDto>();
    }
}

