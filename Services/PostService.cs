using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using SocialMediaApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMediaApp.Services
{
    public class PostService : IPostService
    {
        private readonly IMongoCollection<Post> _posts;

        public PostService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _posts = database.GetCollection<Post>("Posts");
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            return await _posts.Find(post => true).ToListAsync();
        }

        public async Task<Post> GetPostById(string id)
        {
            if (ObjectId.TryParse(id, out ObjectId objectId))
            {
                return await _posts.Find(post => post.Id == objectId).FirstOrDefaultAsync();
            }
            return null;
        }

        public async Task CreatePost(Post post)
        {
            post.CreatedAt = DateTime.UtcNow;  // Set creation timestamp
            await _posts.InsertOneAsync(post);
        }

        public async Task UpdatePost(string id, Post post)
        {
            if (ObjectId.TryParse(id, out ObjectId objectId))
            {
                post.UpdatedAt = DateTime.UtcNow;  // Set updated timestamp
                await _posts.ReplaceOneAsync(p => p.Id == objectId, post);
            }
        }

        public async Task DeletePost(string id)
        {
            if (ObjectId.TryParse(id, out ObjectId objectId))
            {
                await _posts.DeleteOneAsync(p => p.Id == objectId);
            }
        }
    }
}
