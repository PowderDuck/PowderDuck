using Microsoft.EntityFrameworkCore;
using NetAPIExp.Models;

namespace NetAPIExp
{
    public class UserContext : DbContext
    {
        public DbSet<AuthenticationInfo> AuthInfo { get; set; }

        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }
    }
}
