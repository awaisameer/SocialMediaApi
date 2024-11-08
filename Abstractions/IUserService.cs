using SocialMediaApp.Models;

public interface IUserService
{
    Task<User> FindUserByEmail(string email);
    Task AddUser(User user);
    Task<IEnumerable<User>> GetUsers();
    Task<User> GetUserById(string id);
    Task UpdateUser(string id, User user);
    Task DeleteUser(string id);
}
