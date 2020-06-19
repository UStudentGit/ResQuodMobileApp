using ResQuod.Helpers;
using ResQuod.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResQuod.Controllers
{
    public static class SessionController
    {
        private static readonly string userFile = "userSession";
        private static UserSessionData cachedData = null;
        private static bool wasRead = false;

        public static bool IsSaved
        {
            get => GetUserData() != null;
        }

        public static void SaveUserData(UserSessionData user)
        {
            FileHelper.SaveObjectToFile(user, userFile);
        }

        public static UserSessionData GetUserData()
        {
            if (cachedData == null && !wasRead)
            {
                cachedData = FileHelper.ReadObjectFromFile<UserSessionData>(userFile);
                wasRead = true;
            }

            return cachedData;
        }

        public static void ClearUserData()
        {
            FileHelper.ClearFile(userFile);
            cachedData = null;
            wasRead = false;
        }
    }
}
