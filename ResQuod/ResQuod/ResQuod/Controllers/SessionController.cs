using ResQuod.Helpers;
using ResQuod.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResQuod.Controllers
{
    public static class SessionController
    {
        private static string userFile = "userSession";

        public static void SaveUserData(UserSessionData user)
        {
            FileHelper.SaveObjectToFile(user, userFile);
        }

        public static UserSessionData GetUserData()
        {
            var user = FileHelper.ReadObjectFromFile<UserSessionData>(userFile);
            return user;

        }
    }
}
