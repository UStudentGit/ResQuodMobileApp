using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResQuod.Models
{
    public class User
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        public List<Meeting> CreatedMeetings { get; set; }
        public List<Meeting> JoinedMeetings { get; set; }
    }
}
