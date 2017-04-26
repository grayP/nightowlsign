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
    }
}
