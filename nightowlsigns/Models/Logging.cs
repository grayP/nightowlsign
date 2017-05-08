using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nightowlsign.Models
{
    public class Logging
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime DateStamp { get; set; }
 
    }
}