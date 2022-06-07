using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BusinessObject
{
    public class SfbmsDbContext : DbContext
    {
        public SfbmsDbContext(DbContextOptions<SfbmsDbContext> options) : base(options)
        {
        }

        public SfbmsDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("SFBMSDB_CNS")!);
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("SfbmsDb");
            optionsBuilder.UseSqlServer(connectionString);
        }

        public DbSet<Field> Fields { get; set; } = null!;
        public DbSet<Slot> Slots { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<BookingDetail> BookingDetails { get; set; } = null!;
        public DbSet<Feedback> Feedbacks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
