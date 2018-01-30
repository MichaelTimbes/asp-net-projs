using Microsoft.EntityFrameworkCore;

namespace Demo.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
        }

        public DbSet<Demo.Models.UserModel> UserModel { get; set; }
    }
}