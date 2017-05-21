using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nightowlsign.data.Models.Stores
{
    public class StoreDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Suburb { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string Manager { get; set; }
        public string IpAddress { get; set; }
        public string SubMask { get; set; }
        public int Port { get; set; }
        public string ProgramFile { get; set; }
        public int? SignId { get; set; }
        public Sign sign => new Sign() { id = SignId ?? 0 };
    }

}
