using ModelsLib;

namespace REsAPI.Services
{
    public interface ITaskModelServicesInterface
    {
        Task<List<TaskModel>> GetTasksAsync();
        Task<TaskModel?> GetTaskByIdAsync(int id);
        Task<TaskModel> CreateTaskAsync(TaskModel task);
        Task<TaskModel> UpdateTaskAsync(TaskModel task);
        Task<bool> DeleteTaskAsync(int id);
    }
}
