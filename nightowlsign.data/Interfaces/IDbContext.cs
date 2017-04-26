using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nightowlsign.data.Interfaces
{
    public interface IDbContext : IDisposable
    {
        int SaveChanges();

        ObjectResult<GetCurrentPlayList_Result> GetCurrentPlayList();
        ObjectResult<FindCurrentPlayList_Result> FindCurrentPlayList();
        DbSet<Sign> Signs { get; set; }
        DbSet<Store> Store { get; set; }
        DbSet<StoreSign> StoreSigns { get; set; }
        DbSet<Image> Images { get; set; }
        DbSet<Schedule> Schedules { get; set; }
        DbSet<ScheduleStore> ScheduleStores { get; set; }
        DbSet<ScheduleImage> ScheduleImages { get; set; }
        DbSet<ImagesAndSign> ImagesAndSigns { get; set; }
        DbSet<StoreAndSign> StoreAndSigns { get; set; }
        DbSet<ScheduleAndSign> ScheduleAndSigns { get; set; }
        DbSet<StoreScheduleLog> StoreScheduleLogs { get; set; }
    }
}
