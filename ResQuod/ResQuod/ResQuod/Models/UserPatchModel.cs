using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ResQuod.Models
{
    class UserPatchModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }
    }
}
