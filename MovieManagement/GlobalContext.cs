using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManagement
{
    public class GlobalContext
    {
        public static int UserID = 0; // 0 means user did not login yet
        public static bool Go2Setting = false; // Fix Navigate to Setting page after login bug
        public static string seats = "";
        public static int showtimeID = 0;
        public static string voucher = "";
        public static float price = 0;
        public static string name = "";
        public static void SetUserID(int id) {
            UserID = id;
        }

        public static void SetGo2Setting(bool go2Setting) {
            Go2Setting = go2Setting;
        }
        
        public static void SetSeats(string seat)
        {
            seats += seat+ " ";
        }

        public static void removeSeat(string seat)
        {
            seats = seats.Replace(seat + " ", "");
        }

        public static void clearSeats()
        {
            seats = "";
        }

        public static void setShowtime(int ShowtimeID)
        {
            showtimeID = ShowtimeID;
        }

        public static void setVoucher(string value)
        {
            voucher += value + " ";
        }

        public static void removeVoucher(string value)
        {
            voucher = voucher.Replace(value + " ", "");
        }

        public static void clearVoucher()
        {
            voucher = "";
        }

        public static void setPrice(float value)
        {
            price = value;
        }
    }

}
