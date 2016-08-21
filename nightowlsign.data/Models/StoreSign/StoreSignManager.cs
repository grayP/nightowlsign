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
                             select new SignSelect() {Id =s.id, Model = s.Model});
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
                    DateUpdated = DateTime.Now,
                    InstallDate = DateTime.Now,
                    IPAddress = signSelect.IpAddress

                };
                if (signSelect.selected)
                {
                    db.Set<StoreSign>().AddOrUpdate(storeSign);
                    db.SaveChanges();
                }
                else
                {
                    List<StoreSign> storeSigns =
                        db.StoreSigns.Where(
                            x => x.SignId.Value == storeSign.SignId.Value && x.StoreId == storeSign.StoreId).ToList();
                    foreach (StoreSign sign in storeSigns)
                    {
                        db.StoreSigns.Attach(sign);
                        db.StoreSigns.Remove(sign);
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

        internal string GetIpAddress(int signId, int storeId)
        {
            StoreSign storeSign = null;
            using (nightowlsign_Entities db = new nightowlsign_Entities())
            {
                storeSign = db.StoreSigns.FirstOrDefault(x => x.StoreId == storeId && x.SignId == signId);
            }
            return (storeSign != null) ? storeSign.IPAddress : "";
        }


    }

}
