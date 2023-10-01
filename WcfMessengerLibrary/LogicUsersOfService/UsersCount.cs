using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfMessengerLibrary.LogicUsersOfService
{
    internal static class UsersCount
    {
        private static long count = 0;

        public static long getId()
        {
            return count;
        }
        public static void setId()
        {
            count++;
        }

        public static void setNull()
        {
            count = 0;
        }
    }
}
