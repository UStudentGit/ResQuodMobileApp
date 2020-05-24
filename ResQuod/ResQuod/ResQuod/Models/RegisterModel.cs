using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResQuod.Models
{
    class RegisterModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }
    }
}
