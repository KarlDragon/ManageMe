using Microsoft.EntityFrameworkCore;
using UserContent.Models;
namespace UserContent.Models
{
    public class UserContentContext : DbContext
    {
        public UserContentContext(DbContextOptions<UserContentContext> options) : base(options) { }
        public DbSet<UserContent> UserContents { get; set; }
    }
}
