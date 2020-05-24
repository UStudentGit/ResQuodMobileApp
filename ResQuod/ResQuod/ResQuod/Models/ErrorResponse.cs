using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResQuod.Models
{
    class ErrorResponse
    {
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }
    }
}
