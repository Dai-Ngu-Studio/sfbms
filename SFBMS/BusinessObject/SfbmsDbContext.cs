using Microsoft.EntityFrameworkCore;

namespace BusinessObject
{
    public class SfbmsDbContext : DbContext
    {
        public SfbmsDbContext(DbContextOptions<SfbmsDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("SFBMSDB_CNS")!);
        }

        public DbSet<Field> Fields { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
