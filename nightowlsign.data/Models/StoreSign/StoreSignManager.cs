using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace nightowlsign.data.Models
{
    public class StoreSignManager
    {
        public StoreSignManager()
        {
            ValidationErrors = new List<KeyValuePair<string, string>>();

        }
        //Properties
        public List<KeyValuePair<string, string>> ValidationErrors { get; set; }

        public List<int?> Get(Store Entity)
        {
            int storeId = Entity.id;
            using (nightowlsign_Entities db = new nightowlsign_Entities())
            {
                var query = (from c in db.StoreSigns
                             where c.StoreId == storeId
                             select c.SignId);
                return query.ToList();
            }

        }

        public List<SignSelect> GetAllSigns()
        {
            using (nightowlsign_Entities db = new nightowlsign_Entities())
            {
                var query = (from s in db.Signs
                             select new SignSelect() {Id =s.id, Model = s.Model });
                return query.ToList();
            }
        }


        public Store Find(int storeId)
        {
            Store ret = null;
            using (nightowlsign_Entities db = new nightowlsign_Entities())
            {
                ret = db.Store.Find(storeId);
            }
            return ret;

        }

        public void UpdateSignList(SignSelect signSelect, Store store)
        {
            using (nightowlsign_Entities db = new nightowlsign_Entities())
            {
               StoreSign storeSign = new StoreSign()
                {
                    SignId = signSelect.Id,
                    StoreId = store.id,
                    DateUpdated = DateTime.Now

                };
                if (signSelect.selected)
                {
                    db.Set<StoreSign>().AddOrUpdate(storeSign);
                    db.SaveChanges();
                }
                else
                {
                    if (db.StoreSigns.Any(x => x.SignId == storeSign.SignId && x.StoreId == storeSign.StoreId))
                    {
                        db.StoreSigns.Attach(storeSign);
                        db.StoreSigns.Remove(storeSign);
                        db.SaveChanges();
                    }
                }
            }
        }

        internal bool IsSelected(int signId, int storeId)
        {
            bool ret = false;
            using (nightowlsign_Entities db = new nightowlsign_Entities())
            {
                ret = db.StoreSigns.Any(x => x.StoreId == storeId && x.SignId == signId);
            }
            return ret;
        }

       
    }

}
