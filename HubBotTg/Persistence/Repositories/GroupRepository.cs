using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

public class GroupRepository : IGroupRepository
{
    private readonly AppDbContext _context;
    public GroupRepository(AppDbContext context) { _context = context; }

    public async Task<Group?> GetByNameAsync(string name) =>
        await _context.Groups.FirstOrDefaultAsync(g => g.Name == name);

    public async Task GetAllGroupsAsync() =>
        await _context.Groups.ToListAsync();

    public async Task<Group> AddOrGetAsync(string name)
    {
        var group = await GetByNameAsync(name);
        if (group != null) return group;

        group = new Group { Name = name };
        _context.Groups.Add(group);
        await _context.SaveChangesAsync();
        return group;
    }
}
