using Microsoft.EntityFrameworkCore;

namespace Demo.Models
{
    public class UserProfileContext : DbContext
    {
        public UserProfileContext(DbContextOptions<UserProfileContext> options)
            : base(options)
        {
        }

        public DbSet<Demo.Models.UserProfile> UserProfile { get; set; }
    }
}