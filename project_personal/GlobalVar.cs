using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_personal {
    internal class GlobalVar {
        public static SqlConnectionStringBuilder scsb;
        public static string strDBConnectionString = "";
        public static string image_folder = @"C:\iSpan\project_personal\images";
        public static int user_id = 0;
        public static string user_name = "";
        public static int user_level = 0;
        public static int user_status = 0;
        public static string user_realName = "";
        public static string user_phone = "";
        public static string user_email = "";
        public static string user_address = "";
        public static bool hasBirthday = true;
        public static DateTime user_birthday = DateTime.Now;
        public static bool NowSignIn = true; // determine back to signin form or close the entire program
    }
}
