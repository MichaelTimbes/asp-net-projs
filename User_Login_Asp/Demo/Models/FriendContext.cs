using Microsoft.EntityFrameworkCore;

namespace Demo.Models
{
    public class FriendContext : DbContext
    {
       

        public FriendContext(DbContextOptions<FriendContext> options)
            : base(options)
        {
        }

        public DbSet<Demo.Models.FriendModel> FriendModel { get; set; }
    }
}