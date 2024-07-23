namespace ProjectManagerWebAPI.Repository.Interfaces
{
    public interface ITaskRepository
    {
        Task<List<Models.Task>> GetAllAsync();
        Task<Models.Task?> GetAsync(int id);
        Task<Models.Task> CreateAsync(Models.Task task);
        Task<Models.Task?> UpdateAsync(int id, Models.Task task);
        Task<Models.Task?> DeleteAsync(int id);
    }
}
