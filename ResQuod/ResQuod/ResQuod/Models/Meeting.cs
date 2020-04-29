using System;
using System.Collections.Generic;
using System.Text;

namespace ResQuod.Models
{
    class Meeting
    {
        public int MeetingId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public DateTime DateTime { get; set; }


        public User Creator { get; set; }
        public List<User> Members { get; set; }
    }
}
