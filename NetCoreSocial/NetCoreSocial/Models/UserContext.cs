using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetCoreSocial.Models;

namespace NetCoreSocial.Models
{
    public class UserContext : IdentityDbContext<IdentityUser>
    {
        public UserContext(DbContextOptions<UserContext> options)
                : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<NetCoreSocial.Models.UserModel> UserModel { get; set; }
    }
}