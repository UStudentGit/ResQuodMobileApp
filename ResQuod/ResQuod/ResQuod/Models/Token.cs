using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResQuod.Models
{
    class Token
    {
        [JsonProperty("token")]
        public string Value { get; set; }
    }
}
