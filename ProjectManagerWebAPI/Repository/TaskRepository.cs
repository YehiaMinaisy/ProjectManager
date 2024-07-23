using Microsoft.EntityFrameworkCore;
using ProjectManagerWebAPI.Data;
using ProjectManagerWebAPI.Repository.Interfaces;

namespace ProjectManagerWebAPI.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ProjectManagerDatabaseContext _context;

        public TaskRepository(ProjectManagerDatabaseContext context)
        {
            _context = context;
        }

        public async Task<Models.Task> CreateAsync(Models.Task task)
        {
            await _context.AddAsync(task);
            await _context.SaveChangesAsync();
            return task;

        }

        public async Task<Models.Task?> DeleteAsync(int id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);
            if (task == null)
            {
                return null;
            }
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return task;

        }

        public async Task<List<Models.Task>> GetAllAsync()
        {
            return await _context.Tasks
                .Include(t => t.SubTasks)
                .Include(t => t.Project)
                .Include(t => t.TaskUsers)
                .ToListAsync();
        }

        public async Task<Models.Task?> GetAsync(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.SubTasks)
                .Include(t => t.Parent)
                .Include(t => t.Project)
                .Include(t => t.TaskUsers)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (task == null)
            {
                return null;
            }
            return task;
        }

        public async Task<Models.Task?> UpdateAsync(int id, Models.Task task)
        {
            var updatedtask = await _context.Tasks.FindAsync(id);
            if (updatedtask == null)
            {
                return null;
            }
            updatedtask = UpdateTask(updatedtask, task);


            await _context.SaveChangesAsync();
            return updatedtask;

        }
        public static Models.Task UpdateTask(Models.Task task, Models.Task copyTask)
        {
            task.DueDate = copyTask.DueDate;
            task.AttachmentData = copyTask.AttachmentData;
            task.AttachmentName = copyTask.AttachmentName;
            task.AttachmentType = copyTask.AttachmentType;
            task.Name = copyTask.Name;
            task.Description = copyTask.Description;
            task.TaskUsers = copyTask.TaskUsers;
            task.ProjectId = copyTask.ProjectId;
            task.Project = copyTask.Project;
            task.ParentId = copyTask.ParentId;
            task.Parent = copyTask.Parent;
            task.SubTasks = copyTask.SubTasks;
            return task;
        }
    }
}
