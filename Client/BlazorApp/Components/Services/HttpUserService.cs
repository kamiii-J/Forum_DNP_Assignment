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

    public Task<UserDto> UpdateUserAsync(UpdateUserDto request)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> GetUserAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> DeleteUserAsync(int id)
    {
        throw new NotImplementedException();
    }
}