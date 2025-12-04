using BookingService.MVVM.Models;
using System;
using System.Linq;

namespace BookingService.MVVM.ViewModels
{
    public class RoomViewModel : ViewModelBase
    {
        private readonly AppDbContext _context = new AppDbContext();

        private HotelRoom _room;
        public HotelRoom Room { get => _room; set { _room = value; OnPropertyChanged(); OnPropertyChanged(nameof(RoomIsAvailableText)); OnPropertyChanged(nameof(RoomHasBalconyText)); OnPropertyChanged(nameof(RoomIsNonSmokingText)); } }

        public string RoomIsAvailableText => Room != null ? (Room.IsAvailable ? "Да" : "Нет") : string.Empty;
        public string RoomHasBalconyText => Room != null ? (Room.HasBalcony ? "Да" : "Нет") : string.Empty;
        public string RoomIsNonSmokingText => Room != null ? (!Room.IsNonSmoking ? "Да" : "Нет") : string.Empty; // note: display 'Да' if smoking allowed

        public RoomViewModel(int roomId)
        {
            LoadRoom(roomId);
        }

        private void LoadRoom(int roomId)
        {
            try
            {
                Room = _context.HotelRooms.FirstOrDefault(r => r.Id == roomId);
            }
            catch (Exception)
            {
                Room = null;
            }
        }
    }
}