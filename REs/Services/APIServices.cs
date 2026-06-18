using System.Diagnostics;
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
        //получить задачу
        public async Task<List<TaskModel>> GetTasksAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<TaskModel>>("api/tasks") ?? new List<TaskModel>();
        }
        //создать задачу
        public async Task<TaskModel> CreateTaskAsync(TaskModel task)
        {
            try
            {
                var fullUrl = _httpClient.BaseAddress + "api/tasks";
                Debug.WriteLine($"Запрос к: {fullUrl}");
                var response = await _httpClient.PostAsJsonAsync("api/tasks", task);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<TaskModel>();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Server returned {response.StatusCode}: {error}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Network error: {ex.Message}", ex);
            }
        }
        //обновить
        public async Task<bool> UpdateTaskAsync(TaskModel task)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/tasks/{task.Id}", task);
            return response.IsSuccessStatusCode;
        }
        //удалить
        public async Task<bool> DeleteTaskAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/tasks/{id}");
            return response.IsSuccessStatusCode;
        }
        #endregion
    }
}