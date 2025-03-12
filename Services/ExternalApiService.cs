using System.Text.Json;
using System.Text.Json.Serialization;

namespace TaskManagementSystem.Services
{
    public class ExternalApiService
    {
        private readonly HttpClient _httpClient;

        public ExternalApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> IsTaskCompletedAsync(int taskId)
        {
            var response = await _httpClient.GetAsync($"https://jsonplaceholder.typicode.com/todos/{taskId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var todoItem = JsonSerializer.Deserialize<TodoItem>(content);
                return todoItem?.Completed ?? false;
            }
            return false;
        }

        private class TodoItem
        {
            [JsonPropertyName("userId")]
            public int UserId { get; set; }

            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("title")]
            public string Title { get; set; }

            [JsonPropertyName("completed")]
            public bool Completed { get; set; }
        }
    }
}
