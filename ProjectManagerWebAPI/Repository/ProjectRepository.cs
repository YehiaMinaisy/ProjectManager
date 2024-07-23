using Microsoft.EntityFrameworkCore;
using ProjectManagerWebAPI.Data;
using ProjectManagerWebAPI.Dtos.Project;
using ProjectManagerWebAPI.Models;
using ProjectManagerWebAPI.Repository.Interfaces;

namespace ProjectManagerWebAPI.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ProjectManagerDatabaseContext _context;

        public ProjectRepository(ProjectManagerDatabaseContext context)
        {
            _context = context;
        }

        public async Task<Project> CreateAsync(Project project)
        {
            await _context.AddAsync(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task<Project?> DeleteAsync(int id)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
            if (project == null)
            {
                return null;
            }
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task<List<Project>> GetAllAsync()
        {
            var projects = await _context.Projects.ToListAsync();
            return projects;
        }

        public async Task<Project?> GetByIdAsync(int id)
        {
            return await _context.Projects.Include(p => p.Tasks).FirstOrDefaultAsync(p => p.ProjectId == id);
        }

        public Task<bool> ProjectExist(int id)
        {
            return _context.Projects.AnyAsync(p => p.ProjectId == id);
        }

        public async Task<Project?> UpdateAsync(int id, ProjectDto projectDto)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
            if (project == null)
            {
                return null;
            }
            project.ProjectName = projectDto.ProjectName;
            project.ProjectDescription = projectDto.ProjectDescription;
            await _context.SaveChangesAsync();
            return project;
        }

    }
}
