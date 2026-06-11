using System.Net.Http.Json;
using ModelsLib;

namespace REs.Services
{
    public class APIServices
    {
        private readonly HttpClient _httpClient;

        public APIServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        #region методы
        public async Task<List<TaskModel>> GetTasksAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<TaskModel>>("api/tasks") ?? new List<TaskModel>();
        }
        public async Task<bool> CreateTaskAsync(TaskModel task)
        {
            var response = await _httpClient.PostAsJsonAsync("api/tasks", task);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> UpdateTaskAsync(TaskModel task)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/tasks/{task.Id}", task);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteTaskAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/tasks/{id}");
            return response.IsSuccessStatusCode;
        }
        #endregion
    }
}