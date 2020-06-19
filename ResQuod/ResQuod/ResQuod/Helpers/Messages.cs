using System;
using System.Collections.Generic;
using System.Text;

namespace ResQuod.Helpers
{
    public static class ValidationMessages
    {
        public const string PasswordMin = "At least 6 characters required";
        public const string NameMin = "At least 6 characters required";
        public const string IncorrectEmail = "Incorrect email";
        public const string PasswordsNotEqual = "Passwords aren't equal";
    }

    public static class FeedbackMessages
    {
        public const string RequestFail = "Something went wrong! Try again.";
        public const string SaveSuccess = "Data saved successfully!";
    }
}
