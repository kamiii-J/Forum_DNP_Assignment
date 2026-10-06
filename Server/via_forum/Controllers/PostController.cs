using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace via_forum.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;

    public PostsController(IPostRepository postRepo, IUserRepository userRepo)
    {
        this.postRepo = postRepo;
        this.userRepo = userRepo;
    }

    [HttpPost]
    public async Task<ActionResult<PostDto>> AddPost([FromBody] CreatePostDto request)
    {
        try
        {
            // check if user exists
            bool userExists = userRepo.GetMany().Any(u => u.Id == request.UserId);
            if (!userExists) return NotFound($"User with ID {request.UserId} not found.");

            Post post = new Post { Title = request.Title, Body = request.Body, UserId = request.UserId };
            Post created = await postRepo.AddAsync(post);
            
            PostDto dto = new PostDto { Id = created.Id, Title = created.Title, Body = created.Body, UserId = created.UserId };
            return Created($"/Posts/{dto.Id}", dto);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<PostDto>> GetPosts([FromQuery] string? titleContains, [FromQuery] int? userId)
    {
        try
        {
            IQueryable<Post> posts = postRepo.GetMany();

            if (!string.IsNullOrWhiteSpace(titleContains))
            {
                posts = posts.Where(p => p.Title.Contains(titleContains, StringComparison.OrdinalIgnoreCase));
            }
            if (userId.HasValue)
            {
                posts = posts.Where(p => p.UserId == userId.Value);
            }

            List<PostDto> postDtos = posts.Select(p => new PostDto 
            { 
                Id = p.Id, Title = p.Title, Body = p.Body, UserId = p.UserId 
            }).ToList();

            return Ok(postDtos);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PostDto>> GetSingle(int id)
    {
        try
        {
            Post post = await postRepo.GetSingleAsync(id);
            return Ok(new PostDto { Id = post.Id, Title = post.Title, Body = post.Body, UserId = post.UserId });
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePost(int id, [FromBody] UpdatePostDto request)
    {
        try
        {
            Post existingPost = await postRepo.GetSingleAsync(id);

            if (!string.IsNullOrWhiteSpace(request.Title)) existingPost.Title = request.Title;
            if (!string.IsNullOrWhiteSpace(request.Body)) existingPost.Body = request.Body;

            await postRepo.UpdateAsync(existingPost);
            return Ok();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePost(int id)
    {
        try
        {
            await postRepo.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}