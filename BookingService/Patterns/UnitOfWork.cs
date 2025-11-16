using BookingService;
using BookingService.MVVM.Models;
using System;

public class UnitOfWork : IDisposable
{
    private readonly AppDbContext _context = new AppDbContext();

    private IRepository<HotelRoom> _hotelRooms;
    private IRepository<User> _users;

    public IRepository<HotelRoom> HotelRooms =>
        _hotelRooms ??= new Repository<HotelRoom>(_context);

    public IRepository<User> Users =>
        _users ??= new Repository<User>(_context);

    public void Save() => _context.SaveChanges();

    public void Dispose()
    {
        _context.Dispose();
    }
}