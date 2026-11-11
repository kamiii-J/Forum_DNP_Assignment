using System.Security.Claims;
using System.Text.Json;
using ApiContracts;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.StaticFiles.Infrastructure;


namespace BlazorApp.Components.Services;


public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient httpClient;
    
    private ClaimsPrincipal currentUser = new ClaimsPrincipal(new ClaimsIdentity());
    
    public SimpleAuthProvider(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(new AuthenticationState(currentUser));
    }

    public async Task LoginAsync(string userName, string password)
    {
        LoginRequest request = new LoginRequest { Username = userName, Password = password };
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("auth/login", request);

        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Login failed: {error}");
        }

        string responseContent = await response.Content.ReadAsStringAsync();
        UserDto userDto = JsonSerializer.Deserialize<UserDto>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        )!;

        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userDto.UserName),
            new Claim("Id", userDto.Id.ToString())
        };

        ClaimsIdentity identity = new ClaimsIdentity(claims, "apiuth");
        currentUser = new ClaimsPrincipal(identity);
        
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentUser)));
    }
}