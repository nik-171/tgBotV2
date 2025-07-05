using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public long Id { get; set; }

    public string UserId { get; set; }

    public bool IsAdmin { get; set; } = false;

    public bool AdminRoleRequest { get; set; } = false;

    public int? GroupId { get; set; }
    public Group? Group { get; set; }
}
