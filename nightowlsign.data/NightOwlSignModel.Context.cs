﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class nightowlsign_Entities : DbContext
    {
        public nightowlsign_Entities()
            : base("name=nightowlsign_Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Sign> Signs { get; set; }
        public virtual DbSet<Store> Store { get; set; }
        public virtual DbSet<StoreSign> StoreSigns { get; set; }
    }
}
