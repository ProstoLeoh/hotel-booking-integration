using Microsoft.EntityFrameworkCore;
using IntegrationService.Models;

namespace IntegrationService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hotel>()
                .HasIndex(h => h.HotelId)
                .IsUnique();
            
            modelBuilder.Entity<Room>()
                .HasIndex(r => r.RoomId)
                .IsUnique();
            
            modelBuilder.Entity<Booking>()
                .HasIndex(b => b.BookingId)
                .IsUnique();
            
            modelBuilder.Entity<Payment>()
                .HasIndex(p => p.PaymentId)
                .IsUnique();
        }
    }
}
