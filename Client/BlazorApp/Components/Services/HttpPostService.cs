using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Components.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient client;

    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }
    
    public async Task<PostDto> CreateAsync(CreatePostDto request)
    {
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("posts", request);
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<IEnumerable<PostDto>> GetManyAsync(int? id = null, int? userId = null)
    {
        string uri = "posts";
        List<string> queryParams = new();

        if (id.HasValue) queryParams.Add($"id={id.Value}");
        if(userId.HasValue) queryParams.Add($"userId={userId.Value}");

        if (queryParams.Any())
        {
            uri += "?" + string.Join("&", queryParams);
        }
        HttpResponseMessage httpResponse = await client.GetAsync(uri);
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<IEnumerable<PostDto>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<PostDto> GetSingleAsync(int id)
    {
        HttpResponseMessage httpResponse = await client.GetAsync($"post/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception (response);
        }

        return JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
}