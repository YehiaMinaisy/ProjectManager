using ProjectManager.Dtos;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManager.Models
{
    public class ProjectTask
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public int ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public Project? Project { get; set; } 
        [DisplayName("Parent")]
        public int? ParentId { get; set; }

        public ProjectTask? Parent { get; set; }
        public ICollection<ProjectTask>? SubTasks { get; set; } = new List<ProjectTask>();

        [DisplayName("Attachment Name")]
        public string? AttachmentName { get; set; }
        public string? AttachmentType { get; set; }
        public byte[]? AttachmentData { get; set; }
        public virtual ICollection<UserDto> Users { get; set; } = new List<UserDto>();

    }
}
