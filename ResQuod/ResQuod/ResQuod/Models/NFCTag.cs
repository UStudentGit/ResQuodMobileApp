using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ResQuod.Models
{
    [Serializable]
    class NFCTag
    {
        public string TagId { get; set; }
        public string MeetingCode { get; set; }
    }
}
