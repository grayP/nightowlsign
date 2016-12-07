using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace nightowlsign.data.Models.Stores
{
    public class StoreManager
    {
        public StoreManager()
        {
            ValidationErrors = new List<KeyValuePair<string, string>>();
        }
        //Properties
        public List<KeyValuePair<string, string>> ValidationErrors { get; set; }

        public List<StoreAndSign> Get(Store Entity)
        {
            using (nightowlsign_Entities db = new nightowlsign_Entities())
            {
                var ret = db.StoreAndSigns.OrderBy(x => x.Name).ToList<StoreAndSign>();
                if (!string.IsNullOrEmpty(Entity.Name))
                {
                   ret= ret.FindAll(p => p.Name.ToLower().StartsWith(Entity.Name));
                }
                return ret;
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

        public bool Validate(Store entity)
        {
            ValidationErrors.Clear();
            if (!string.IsNullOrEmpty(entity.Name))
            {
                if (entity.Name.ToLower() == entity.Name)
                {
                    ValidationErrors.Add(new KeyValuePair<string, string>("Store Name", "Store Name cannot be all lower case"));
                }
            }
            return (ValidationErrors.Count == 0);
        }


        public Boolean Update(Store entity)
        {
            bool ret = false;
            if (Validate(entity))
            {
                try
                {
                    using (nightowlsign_Entities db = new nightowlsign_Entities())
                    {
                        db.Store.Attach(entity);
                        var modifiedStore = db.Entry(entity);
                        modifiedStore.Property(e => e.Name).IsModified = true;
                        modifiedStore.Property(e => e.Address).IsModified = true;
                        modifiedStore.Property(e => e.Suburb).IsModified = true;
                        modifiedStore.Property(e => e.State).IsModified = true;
                        modifiedStore.Property(e => e.Manager).IsModified = true;
                        modifiedStore.Property(e => e.Phone).IsModified = true;
                        modifiedStore.Property(e => e.SignId).IsModified = true;
                        modifiedStore.Property(e => e.IpAddress).IsModified = true;
                        modifiedStore.Property(e => e.SubMask).IsModified = true;
                        modifiedStore.Property(e => e.Port).IsModified = true;
                        db.SaveChanges();
                        ret = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                    ret = false;
                }
            }
            return ret;
        }

        public Boolean Insert(Store entity)
        {
            bool ret = false;
            try
            {
                ret = Validate(entity);
                if (ret)
                {
                    using (nightowlsign_Entities db = new nightowlsign_Entities())
                    {
                        Store newStore = new Store()
                        {
                            Name = entity.Name.Trim(),
                            Address = entity.Address,
                            Suburb = entity.Suburb,
                            State = entity.State,
                            Manager = entity.Manager,
                            Phone = entity.Phone,
                            SignId = entity.SignId,
                            IpAddress = entity.IpAddress,
                            SubMask = entity.SubMask,
                            Port = entity.Port
                        };

                        db.Store.Add(newStore);
                        db.SaveChanges();
                        ret = true;
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return ret;
            }
        }


        public bool Delete(Store entity)
        {
            bool ret = false;
            using (nightowlsign_Entities db = new nightowlsign_Entities())
            {
                db.Store.Attach(entity);
                db.Store.Remove(entity);
                db.SaveChanges();
                ret = true;
            }
            return ret;
        }
    }

}
