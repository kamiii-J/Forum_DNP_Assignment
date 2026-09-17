using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    public List<Post> posts = [];

    public PostInMemoryRepository()
    {
        posts.Add(new Post
        {
            Id = 1,
            UserId = 1,
            Title = "Welcome to the new forum!",
            Body = "I am so glad this CLI forum is finally working."
        });

        posts.Add(new Post
        {
            Id = 2,
            UserId = 2,
            Title = "Need help with C#",
            Body = "Does anyone know how Dependency Injection actually works?"
        });

        posts.Add(new Post
        {
            Id = 3,
            UserId = 1,
            Title = "Weekend Plans",
            Body = "Who is doing what this weekend? I am just coding."
        });
    }

    public Task<Post> AddAsync(Post post)
    {
        post.Id = posts.Any()
            ? posts.Max(p => p.Id) + 1
            : 1;
        posts.Add(post);
        return Task.FromResult(post);
    }

    public Task UpdateAsync(Post post)
    {
        Post? existingPost = posts.SingleOrDefault(p => p.Id == post.Id);
        if (existingPost is null)
        {
            throw new InvalidOperationException($"Post not found with id '{post.Id}' not found");
        }

        posts.Remove(existingPost);
        posts.Add(post);

        return Task.CompletedTask;
    }
    
    public Task DeleteAsync(int id)
    {
        Post? postToRemove = posts.SingleOrDefault(p => p.Id == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        posts.Remove(postToRemove);
        return Task.CompletedTask;
    }
    
    public Task<Post> GetSingleAsync(int id)
    {
        var post = posts.SingleOrDefault(p => p.Id == id);
        if (post is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found");
        }

        return Task.FromResult(post);
    }
    
    public IQueryable<Post> GetMany()
    {
        return posts.AsQueryable();
    }
}