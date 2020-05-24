using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace ResQuod.Controllers
{
    class InternetController
    {
        public static bool IsInternetActive()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                return true;
            }

            return false;
        }
    }
}
