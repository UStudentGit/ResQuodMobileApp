using System;
using System.Collections.Generic;
using System.Text;

namespace ResQuod.Models
{
    class User
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public List<Meeting> CreatedMeetings { get; set; }
        public List<Meeting> JoinedMeetings { get; set; }
    }
}
