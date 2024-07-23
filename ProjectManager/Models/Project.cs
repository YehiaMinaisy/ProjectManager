using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        [MaxLength(100)]
        [Required]
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
    }
}
