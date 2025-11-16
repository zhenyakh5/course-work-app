using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingService
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsBlocked { get; set; }

        [NotMapped]
        public bool IsAdmin
        {
            get => RoleId == 1;
            set => RoleId = value ? 1 : 0;
        }

        public User()
        {
            CreatedAt = DateTime.Now;
            IsBlocked = false;
            RoleId = 0;
        }
    }
}