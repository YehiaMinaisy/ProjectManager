using ProjectManagerWebAPI.Dtos.Project;
using ProjectManagerWebAPI.Models;

namespace ProjectManagerWebAPI.Repository.Interfaces
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllAsync();
        Task<Project?> GetByIdAsync(int id);
        Task<Project> CreateAsync(Project project);
        Task<Project?> UpdateAsync(int id, ProjectDto projectDto);

        Task<Project?> DeleteAsync(int id);
        Task<bool> ProjectExist(int id);

    }
}
