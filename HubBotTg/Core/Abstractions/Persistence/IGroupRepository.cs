public interface IGroupRepository
{
    Task GetAllGroupsAsync();
    Task<Group?> GetByNameAsync(string name);
    Task<Group> AddOrGetAsync(string name);
}