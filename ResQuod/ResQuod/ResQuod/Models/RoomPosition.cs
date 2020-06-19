using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResQuod.Models
{
    class RoomPosition
    {
        [JsonProperty("id")]
        public int PositionId { get; set; }

        [JsonProperty("numberOfPosition")]
        public int PositionNumber { get; set; }

        [JsonProperty("roomId")]
        public int RoomId { get; set; }

        [JsonProperty("roomName")]
        public string RoomName { get; set; }
    }
}
