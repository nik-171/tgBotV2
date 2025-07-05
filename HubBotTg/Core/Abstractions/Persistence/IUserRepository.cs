public interface IUserRepository
{
    Task<User?> GetByIdAsync(long id);
    Task UpsertUserAsync(User user, string groupName);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(long userId);
    Task<List<User>> GetUsersByGroupNameAsync(string? groupName);
    Task<List<User>> GetUsersByAdminRequestAsync(bool adminRequest);
}
