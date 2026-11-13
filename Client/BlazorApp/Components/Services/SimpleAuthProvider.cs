using System.Security.Claims;
using System.Text.Json;
using ApiContracts;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.StaticFiles.Infrastructure;
using Microsoft.JSInterop;


namespace BlazorApp.Components.Services;


public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient httpClient;
    private readonly IJSRuntime jsRuntime;
    
    private ClaimsPrincipal currentUser;
    
    public SimpleAuthProvider(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        this.httpClient = httpClient;
        this.jsRuntime = jsRuntime;
    }
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string userAsJson = await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
        try
        {
            userAsJson = await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
        }
        catch (InvalidOperationException e)
        {
            return new AuthenticationState(new());
        }

        if (string.IsNullOrEmpty(userAsJson))
        {
            return new AuthenticationState(new());
        }

        UserDto userDto = JsonSerializer.Deserialize<UserDto>(userAsJson)!;
        List<Claim> claims = new List<Claim>();
        {
            new Claim(ClaimTypes.Name, userDto.UserName);
            new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString());
        }
        ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth");
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
        return new AuthenticationState(claimsPrincipal);
    }
    
    public async Task Logout()
    {
        await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", "");
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new())));
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
        
        string serialisedData = JsonSerializer.Serialize(userDto);
        await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", serialisedData);

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