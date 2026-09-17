using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository
{
    private readonly List<Comment> comments = [];
    
    public CommentInMemoryRepository()
    {
        comments.Add(new Comment 
        { 
            Id = 1, 
            PostId = 1, 
            UserId = 2, 
            Body = "Thanks Alice! Looks great." 
        });
        
        comments.Add(new Comment 
        { 
            Id = 2, 
            PostId = 2, 
            UserId = 1, 
            Body = "Check the Microsoft Docs, they explain it well." 
        });
        
        comments.Add(new Comment 
        { 
            Id = 3, 
            PostId = 2, 
            UserId = 3, 
            Body = "I can help you with that tomorrow!" 
        });
        
        comments.Add(new Comment 
        { 
            Id = 4, 
            PostId = 3, 
            UserId = 2, 
            Body = "I'm also just studying all weekend." 
        });
    }
    public Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = comments.Any() ? comments.Max(c => c.Id) + 1 : 1;
        comments.Add(comment);
        return Task.FromResult(comment); 
    }

    public Task UpdateAsync(Comment comment)
    {
        Comment? existingComment = comments.SingleOrDefault(c => c.Id == comment.Id); // find a single matching comment, or return null.
        if (existingComment is null)
        {
            throw new InvalidOperationException($"Comment with ID '{comment.Id}' not found");
        }
        comments.Remove(existingComment);
        comments.Add(comment);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        var commentToRemove = comments.SingleOrDefault(c => c.Id == id);
        if (commentToRemove is null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found");
        }

        comments.Remove(commentToRemove);
        return Task.CompletedTask;
    }

    public Task<Comment> GetSingleAsync(int id)
    {
        var comment = comments.SingleOrDefault(u => u.Id == id);
        if (comment is null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found");
        }

        return Task.FromResult(comment);
    }

    public IQueryable<Comment> GetMany()
    {
        return comments.AsQueryable();
    }
}