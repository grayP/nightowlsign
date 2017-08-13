using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nightowlsign.data.Models.Brightness
{
    public class BrightnessManager
    {
        public BrightnessManager()
        {

        }

        public List<SignBrightness> Get(int signId)
        {
            using (var db = new nightowlsign_Entities())
            {
                return db.SignBrightnesses.Where(x => x.SignId == signId).OrderBy(x=>x.Period).ToList<SignBrightness>();
            }

        }
    }
}
