using ModelsLib;

namespace REsAPI.Services
{
    public interface ITaskModelServicesInterface
    {
        Task<List<TaskModel>> GetTasksAsync();
        Task<List<TaskModel>> GetPendingTasksAsync();
        Task<List<TaskModel>> GetReadyTasksAsync();
        Task<TaskModel?> GetTaskByIdAsync(int id);
        Task<TaskModel> CreateTaskAsync(TaskModel task);
        Task<TaskModel> UpdateTaskAsync(TaskModel task);
        Task<bool> DeleteTaskAsync(int id);
    }
}
