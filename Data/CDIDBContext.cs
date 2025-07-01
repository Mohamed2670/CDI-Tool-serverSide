using CDI_Tool.Model;
using Microsoft.EntityFrameworkCore;

namespace CDI_Tool.Data
{
    public class CDIDBContext : DbContext
    {
        public CDIDBContext(DbContextOptions<CDIDBContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Log>Logs { get; set; }
    }
}