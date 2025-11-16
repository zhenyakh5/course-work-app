using BookingService.MVVM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingService.Services
{
    public interface IRoomService
    {
        Task<List<HotelRoom>> GetAllRoomsAsync();
        Task<List<HotelRoom>> SearchRoomsAsync(string searchText);
        Task<HotelRoom> GetRoomByIdAsync(int id);
        Task<bool> AddRoomAsync(HotelRoom room);
        Task<bool> UpdateRoomAsync(HotelRoom room);
        Task<bool> DeleteRoomAsync(int roomId);
    }
}