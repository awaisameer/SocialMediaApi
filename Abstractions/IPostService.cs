using SocialMediaApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMediaApp.Services
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetPosts();  // Get all posts
        Task<Post> GetPostById(string id);  // Get a post by Id
        Task CreatePost(Post post);  // Create a new post
        Task UpdatePost(string id, Post post);  // Update a post
        Task DeletePost(string id);  // Delete a post
    }
}
