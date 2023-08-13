using cmdAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace cmdAPI.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Command> Commands => Set<Command>();
    }
}