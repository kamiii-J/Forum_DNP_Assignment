using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class SinglePostView
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;
    private  readonly IUserRepository userRepository;
    
    public SinglePostView(IPostRepository postRepository, ICommentRepository commentRepository, IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
    }

    public async Task ShowAsync()
    {
        Console.Write("Enter Post ID to view: ");
        if (!int.TryParse(Console.ReadLine(), out int postId)) return;

        try
        {
            var post = await postRepository.GetSingleAsync(postId);
            
            Console.WriteLine($"\n=== {post.Title} ===");
            Console.WriteLine(post.Body);
            Console.WriteLine($"Author User ID: {post.UserId}");
            Console.WriteLine("---------------------");
            Console.WriteLine("Comments:");
            
            var allComments = commentRepository.GetMany().ToList();
            bool hasComments = false;

            foreach (var c in allComments)
            {
                if (c.PostId == postId)
                {
                    Console.WriteLine($"  - {c.Body} (User ID: {c.UserId})");
                    hasComments = true;
                }
            }

            if (!hasComments)
            {
                Console.WriteLine("  (No comments yet)");
            }

            Console.WriteLine("\n[1] Add a comment");
            Console.WriteLine("[2] Go back");
            if (Console.ReadLine() == "1")
            {
                Console.Write("Enter your User ID: ");
                if (!int.TryParse(Console.ReadLine(), out int userId))
                {
                    Console.WriteLine("Error: Invalid ID format.");
                    return;
                }
                
                bool userExists = userRepository.GetMany().Any(u => u.Id == userId);
                if (!userExists)
                {
                    Console.WriteLine($"Error: User with ID '{userId}' not found. Cannot add comment.");
                    return;
                }

                Console.Write("Enter comment body: ");
                string body = Console.ReadLine() ?? "";
                
                if (string.IsNullOrWhiteSpace(body))
                {
                    Console.WriteLine("Error: Comment cannot be empty.");
                    return;
                }

                await commentRepository.AddAsync(new Entities.Comment 
                { 
                    PostId = postId, 
                    UserId = userId, 
                    Body = body 
                });
                
                Console.WriteLine("Comment added!");
            }
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }
}