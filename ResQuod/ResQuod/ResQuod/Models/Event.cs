using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ResQuod.Models
{
    class Event
    {
        [JsonProperty("administratorID")]
        public string AdminId { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("room")]
        public Room Room { get; set; }
    }
}
