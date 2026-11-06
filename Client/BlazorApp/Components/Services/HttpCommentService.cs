using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Components.Services;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient client;
    
    public HttpCommentService(HttpClient client)
    {
        this.client = client;
    }
    public async Task<CommentDto> CreateAsync(CreateCommentDto request)
    {
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("comments", request);
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<CommentDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
    
    public async Task<IEnumerable<CommentDto>> GetCommentsAsync(int? postId = null, int? userId = null)
    {
        string uri = "comments";
        List<string> queryParams = new();
        
        if(postId.HasValue) queryParams.Add($"postID={postId.Value}");
        if(userId.HasValue) queryParams.Add($"userID={userId.Value}");

        if (queryParams.Any())
        {
            uri += "?" + string.Join("&", queryParams);
        }
        
        HttpResponseMessage httpResponse = await client.GetAsync(uri);
        string response = await httpResponse.Content.ReadAsStringAsync();
        
        if(!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<IEnumerable<CommentDto>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;

    }

    public async Task<CommentDto> GetSingleAsync(int id)
    {
        HttpResponseMessage httpResponse = await client.GetAsync($"comments/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<CommentDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
}