using ApiContracts;

namespace BlazorApp.Components.Services;

public interface IPostService
{
    Task<PostDto> CreateAsync(CreatePostDto request);
    Task<IEnumerable<PostDto>> GetManyAsync(int? id = null, int? userId = null);
    Task<PostDto> GetSingleAsync(int id);
}