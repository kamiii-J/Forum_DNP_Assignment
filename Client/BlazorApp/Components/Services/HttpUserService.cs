using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Components.Services;

public class HttpUserService : IUserService
{
    private readonly  HttpClient httpClient;

    public HttpUserService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<UserDto> AddUserAsync(CreateUserDto request)
    {
        HttpResponseMessage httpResponse = await httpClient.PostAsJsonAsync("users", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<UserDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task UpdateUserAsync(int id, UpdateUserDto request)
    {
        HttpResponseMessage httpResponse = await httpClient.PutAsJsonAsync($"users/{id}", request);
        
        if (!httpResponse.IsSuccessStatusCode)
        {
            string response = await httpResponse.Content.ReadAsStringAsync();
            throw new Exception(response);
        }
    }

    public async Task<IEnumerable<UserDto>> GetUserAsync(int? id = null)
    {
        string uri = "user";
        if (id.HasValue)
        {
            uri += $"?id={id.Value}";
        }

        HttpResponseMessage httpResponse = await httpClient.GetAsync(uri);
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<IEnumerable<UserDto>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
}