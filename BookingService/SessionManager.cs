using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService
{
    public static class SessionManager
    {
        public static User? CurrentUser { get; private set; }

        public static void SetCurrentUser(User user)
        {
            CurrentUser = user;
        }
    }
}
