using Microsoft.EntityFrameworkCore;
using ModelsLib;
using ModelsLib.Data;

namespace REsAPI.Services
{
    public class TaskService : ITaskModelServicesInterface
    {
        private readonly ApplicationDbContext _context;
        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<TaskModel>> GetTasksAsync()
        {
            return await _context.Tasks.OrderByDescending(t => t.Created).ToListAsync();
        }
        public async Task<TaskModel?> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }
        public async Task<TaskModel> CreateTaskAsync(TaskModel task)
        {
            task.Created = DateTime.Now;
            task.Ready = false;
            task.Ended = null;

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }
        //прверить 
        public async Task<TaskModel> UpdateTaskAsync(TaskModel task)
        {
            var existing = await _context.Tasks.FindAsync(task.Id);
            if (existing == null) throw new KeyNotFoundException("...");

            bool wasReady = existing.Ready;
            existing.Name = task.Name;
            existing.Ready = task.Ready;
            
            if (task.Deadline.HasValue) existing.Deadline = task.Deadline;

            if (task.Ready && !wasReady) existing.Ended = DateTime.UtcNow;
            else if (!task.Ready) existing.Ended = null;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}