using BookingService.MVVM.Models;
using BookingService.MVVM.Services;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BookingService.Services
{
    public class RoomService : IRoomService
    {
        private readonly AppDbContext _context;

        public RoomService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<HotelRoom>> GetAllRoomsAsync()
        {
            return await _context.HotelRooms.ToListAsync();
        }

        public async Task<List<HotelRoom>> SearchRoomsAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return new List<HotelRoom>();

            return await _context.HotelRooms
                .Where(r => r.Name.Contains(searchText) || r.ShortDescription.Contains(searchText))
                .ToListAsync();
        }

        public async Task<HotelRoom> GetRoomByIdAsync(int id)
        {
            return await _context.HotelRooms.FindAsync(id);
        }

        public async Task<bool> AddRoomAsync(HotelRoom room)
        {
            try
            {
                _context.HotelRooms.Add(room);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateRoomAsync(HotelRoom room)
        {
            try
            {
                var existingRoom = await _context.HotelRooms.FindAsync(room.Id);
                if (existingRoom == null)
                    return false;

                _context.Entry(existingRoom).CurrentValues.SetValues(room);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteRoomAsync(int roomId)
        {
            try
            {
                var room = await _context.HotelRooms.FindAsync(roomId);
                if (room != null)
                {
                    _context.HotelRooms.Remove(room);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}