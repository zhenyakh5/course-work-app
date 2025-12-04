using BookingService.MVVM.Models;
using System;
using System.Data.Entity;

namespace BookingService
{
    public class AppDbContext : DbContext
    {
        public event Action DataChanged;

        public DbSet<User> Users { get; set; }
        public DbSet<HotelRoom> HotelRooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public AppDbContext() : base("Server=localhost; Database=HotelDatabase; TrustServerCertificate=True; Integrated Security=True;")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public void NotifyDataChanged()
        {
            DataChanged?.Invoke();
        }
    }
}