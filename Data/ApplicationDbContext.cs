using Microsoft.EntityFrameworkCore;
using ResourceBookingSystem.Models; // Adjust if your models are in a different namespace

namespace ResourceBookingSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Resource> Resources { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Resource)
                .WithMany() // or .WithMany(r => r.Bookings) if you have a collection
                .HasForeignKey(b => b.ResourceId)
                .OnDelete(DeleteBehavior.Restrict); // 👈 Prevent deleting the resource
        }

    }
}
