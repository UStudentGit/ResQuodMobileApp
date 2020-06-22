using System;
using System.Collections.Generic;
using System.Text;

namespace ResQuod.Models
{
    class Constants
    {
        public static readonly string API_LoginUrl = "http://whcp.pl:3200/login";
        public static readonly string API_RegisternUrl = "http://whcp.pl:3200/register";
        public static readonly string API_GetUserUrl = "http://whcp.pl:3200/user";
        public static readonly string API_ReportPresence = "http://whcp.pl:3200/presenceAtPosition";
        public static readonly string API_UserPatchUrl = "http://whcp.pl:3200/userPatch";
        public static readonly string API_GetNullTagPositions = "http://whcp.pl:3200/nullTagsGetter?id=1";
        public static readonly string API_AssignTagToPosition = "http://whcp.pl:3200/tagIdSetter";
        public static readonly string API_SendPresence = "http://whcp.pl:3200/presenceAtPosition";
        public static readonly string API_JoinEvent = "http://whcp.pl:3200/toEvent";
        public static readonly string API_GetUserEvents = "http://whcp.pl:3200/userEvents";
    }
}
