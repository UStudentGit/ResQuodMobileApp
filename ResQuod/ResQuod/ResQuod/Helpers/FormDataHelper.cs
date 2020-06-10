using System;
using System.Collections.Generic;
using System.Text;

namespace ResQuod
{
    class FormDataHelper
    {
        public static bool IsEmailValid(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
