using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace via_forum.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepo;
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;

    public CommentsController(ICommentRepository commentRepo, IPostRepository postRepo, IUserRepository userRepo)
    {
        this.commentRepo = commentRepo;
        this.postRepo = postRepo;
        this.userRepo = userRepo;
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> AddComment([FromBody] CreateCommentDto request)
    {
        try
        {
            // Verify if user and post exist
            bool userExists = userRepo.GetMany().Any(u => u.Id == request.UserId);
            if (!userExists) return NotFound($"User with ID {request.UserId} not found.");

            bool postExists = postRepo.GetMany().Any(p => p.Id == request.PostId);
            if (!postExists) return NotFound($"Post with ID {request.PostId} not found.");

            Comment comment = new Comment { Body = request.Body, UserId = request.UserId, PostId = request.PostId };
            Comment created = await commentRepo.AddAsync(comment);
            
            CommentDto dto = new CommentDto { Id = created.Id, Body = created.Body, UserId = created.UserId, PostId = created.PostId };
            return Created($"/Comments/{dto.Id}", dto);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommentDto>> GetComments([FromQuery] int? userId, [FromQuery] int? postId)
    {
        try
        {
            IQueryable<Comment> comments = commentRepo.GetMany();

            if (userId.HasValue) comments = comments.Where(c => c.UserId == userId.Value);
            if (postId.HasValue) comments = comments.Where(c => c.PostId == postId.Value);

            List<CommentDto> commentDtos = comments.Select(c => new CommentDto 
            { 
                Id = c.Id, Body = c.Body, UserId = c.UserId, PostId = c.PostId 
            }).ToList();

            return Ok(commentDtos);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetSingle(int id)
    {
        try
        {
            Comment comment = await commentRepo.GetSingleAsync(id);
            return Ok(new CommentDto { Id = comment.Id, Body = comment.Body, UserId = comment.UserId, PostId = comment.PostId });
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
    public async Task<ActionResult> UpdateComment(int id, [FromBody] UpdateCommentDto request)
    {
        try
        {
            Comment existingComment = await commentRepo.GetSingleAsync(id);
            
            if (!string.IsNullOrWhiteSpace(request.Body)) existingComment.Body = request.Body;
            
            await commentRepo.UpdateAsync(existingComment);
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
    public async Task<ActionResult> DeleteComment(int id)
    {
        try
        {
            await commentRepo.DeleteAsync(id);
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