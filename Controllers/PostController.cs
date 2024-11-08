using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using SocialMediaApp.Models;
using SocialMediaApp.Services;

namespace SocialMediaApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [Authorize] // Ensures the user is authenticated
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] Post post)
        {
            // Extract user ID from JWT token
            var userIdClaim = User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("User ID is missing in the token");
            }

            // Parse the user ID
            ObjectId userId;
            try
            {
                userId = ObjectId.Parse(userIdClaim);
            }
            catch (FormatException)
            {
                return Unauthorized("Invalid User ID format in token");
            }

            post.UserId = userId; // Link the post to the user
            post.CreatedAt = DateTime.UtcNow; // Set the creation time

            await _postService.CreatePost(post); // Save the post to MongoDB

            return Ok("Post created successfully");
        }

     

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(string id, [FromBody] Post post)
        {
            await _postService.UpdatePost(id, post);
            return Ok("Post updated successfully");
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(string id)
        {
            await _postService.DeletePost(id);
            return Ok("Post deleted successfully");
        }
    }
}
