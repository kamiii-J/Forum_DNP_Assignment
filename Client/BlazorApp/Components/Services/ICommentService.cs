using ApiContracts;

namespace BlazorApp.Components.Services;

public interface ICommentService
{
    Task<CommentDto> CreateAsync(CreateCommentDto request);
    Task<IEnumerable<CommentDto>> GetCommentsAsync(int? postId = null, int? userId = null);
    Task<CommentDto> GetSingleAsync(int id);
}