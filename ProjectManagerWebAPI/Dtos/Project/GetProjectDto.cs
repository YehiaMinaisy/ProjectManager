using ProjectManagerWebAPI.Dtos.Task;

namespace ProjectManagerWebAPI.Dtos.Project
{
    public class GetProjectDto
    {

        public string ProjectName { get; set; } = null!;

        public string ProjectDescription { get; set; } = null!;
        public virtual ICollection<TaskDto> Tasks { get; set; } = new List<TaskDto>();
    }
}
