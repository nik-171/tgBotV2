using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HubBotTg.Core.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}