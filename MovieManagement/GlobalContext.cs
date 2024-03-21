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

        public static void SetUserID(int id) {
            UserID = id;
        }

        public static void SetGo2Setting(bool go2Setting) {
            Go2Setting = go2Setting;
        }
    }
}
