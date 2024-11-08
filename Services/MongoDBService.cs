using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using SocialMediaApp.Models;

namespace SocialMediaApp.Services
{
    public class MongoDBService : IUserService
    {
        private readonly IMongoCollection<User> _users;

        public MongoDBService(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _users = database.GetCollection<User>("Users");
        }

        // Method to find a user by email
        public async Task<User> FindUserByEmail(string email)
        {
            return await _users.Find(user => user.Email == email).FirstOrDefaultAsync();
        }

        // Method to add a new user
        public async Task AddUser(User user)
        {
            await _users.InsertOneAsync(user);
        }

        // Other methods (GetUsers, GetUserById, UpdateUser, DeleteUser) can remain unchanged
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _users.Find(user => true).ToListAsync();
        }

        public async Task<User> GetUserById(string id)
        {
            if (ObjectId.TryParse(id, out ObjectId objectId))
            {
                return await _users.Find(user => user.Id == objectId).FirstOrDefaultAsync();
            }
            return null;
        }

        public async Task UpdateUser(string id, User user)
        {
            if (ObjectId.TryParse(id, out ObjectId objectId))
            {
                await _users.ReplaceOneAsync(u => u.Id == objectId, user);
            }
        }

        public async Task DeleteUser(string id)
        {
            if (ObjectId.TryParse(id, out ObjectId objectId))
            {
                await _users.DeleteOneAsync(u => u.Id == objectId);
            }
        }
    }
}