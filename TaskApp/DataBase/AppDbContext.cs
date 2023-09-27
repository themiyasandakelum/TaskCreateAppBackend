using Microsoft.EntityFrameworkCore;

namespace TaskApp.DataBase
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options ) :base (options) { }
    }
}
