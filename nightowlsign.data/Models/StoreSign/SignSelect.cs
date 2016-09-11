﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nightowlsign.data.Models
{
    public class SignSelect
    {
        public bool selected { get; set; }
        public int Id { get; set; }
        public int? StoreId { get; set; }
        public int? SignId { get; set; }
        public string Model { get; set; }
        public string IpAddress  { get; set; }
    }
}
