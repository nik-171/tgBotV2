using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(long id)
    {
        return await _context.Users
            .Include(u => u.Group)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<List<User>> GetUsersByGroupNameAsync(string? groupName)
    {
        if (string.IsNullOrEmpty(groupName))
            {
                return await _context.Users
                    .Include(u => u.Group)
                    .ToListAsync();
            }
        else
            {
                return await _context.Users
                    .Include(u => u.Group)
                    .Where(u => u.Group != null && u.Group.Name == groupName)
                    .ToListAsync();
            }
    }
    public async Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetUsersByAdminRequestAsync(bool adminRequest)
    {
        return await _context.Users
            .Where(u => u.AdminRoleRequest == adminRequest)
            .ToListAsync();
    }

    public async Task UpsertUserAsync(User user, string groupName)
    {
        var group = await _context.Groups
            .FirstOrDefaultAsync(g => g.Name == groupName);

        if (group == null)
        {
            group = new Group { Name = groupName };
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
        }

        user.GroupId = group.Id;

        var existing = await _context.Users.FindAsync(user.Id);
        if (existing == null)
        {
            _context.Users.Add(user);
        }
        else
        {
            existing.FirstName = user.FirstName;
            existing.LastName = user.LastName;
            existing.PhoneNumber = user.PhoneNumber;
            existing.IsAdmin = user.IsAdmin;
            existing.AdminRoleRequest = user.AdminRoleRequest;
            existing.GroupId = user.GroupId;
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(long id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
