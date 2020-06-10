using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResQuod.Models
{
    public class PresenceResponse
    {
        [JsonProperty("eventId")]
        public int EventId { get; set; }

        [JsonProperty("firstResponse")]
        public bool FirstResponse { get; set; }

        [JsonProperty("eventName")]
        public string EventName { get; set; }

        [JsonProperty("startTime")]
        public DateTime StartTime { get; set; }

        [JsonProperty("endTime")]
        public DateTime EndTime { get; set; }
    }
}
