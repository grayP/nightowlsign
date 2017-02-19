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
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
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
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<ScheduleStore> ScheduleStores { get; set; }
        public virtual DbSet<ScheduleImage> ScheduleImages { get; set; }
        public virtual DbSet<ScheduleSign> ScheduleSigns { get; set; }
        public virtual DbSet<ImagesAndSign> ImagesAndSigns { get; set; }
        public virtual DbSet<LastInstalledSchedule> LastInstalledSchedules { get; set; }
        public virtual DbSet<StoreAndSign> StoreAndSigns { get; set; }
        public virtual DbSet<ScheduleAndSign> ScheduleAndSigns { get; set; }
    
        public virtual ObjectResult<GetCurrentPlayList_Result> GetCurrentPlayList()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetCurrentPlayList_Result>("GetCurrentPlayList");
        }
    }
}
