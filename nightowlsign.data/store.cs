//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace nightowlsign.data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Store
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Suburb { get; set; }
        public string State { get; set; }
        public Nullable<decimal> Latitude { get; set; }
        public Nullable<decimal> Longitude { get; set; }
        public string Manager { get; set; }
        public string Phone { get; set; }
        public Nullable<int> SignId { get; set; }
        public string IpAddress { get; set; }
        public string SubMask { get; set; }
        public string Port { get; set; }
        public string ProgramFile { get; set; }
        public Nullable<int> LastUpdateStatus { get; set; }
        public Nullable<System.DateTime> LastUpdateTime { get; set; }
        public Nullable<int> NumImages { get; set; }
    }
}
