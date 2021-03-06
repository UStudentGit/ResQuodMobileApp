﻿using ResQuod.Helpers;
using ResQuod.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

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
            if (!string.IsNullOrEmpty(user?.Password))
            {
                user.Password = EncryptionHelper.EncryptWithAes(user.Password);
            }
            FileHelper.SaveObjectToFile(user, userFile);
        }

        public static UserSessionData GetUserData()
        {
            if (cachedData == null && !wasRead)
            {
                cachedData = FileHelper.ReadObjectFromFile<UserSessionData>(userFile);
                if (!string.IsNullOrEmpty(cachedData?.Password))
                {
                    try
                    {
                        cachedData.Password = EncryptionHelper.DecryptFromAes(cachedData.Password);
                    }
                    catch (Exception e)
                    {
                        return null;
                    }
                }
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
