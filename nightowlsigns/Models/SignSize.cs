using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nightowlsign.Models
{
    public class SignSize
    {
        public int SignId { get; set; }

    }
    public static class Helper
    {
        public static SignSize GetMySessionObject(this HttpContext current)
        {
            return current != null ? (SignSize)current.Session["__SessionSignSize"] : null;
        }



        internal static int GetSetSignSize(int? signId)
        {
            SignSize signSize = GetMySessionObject(HttpContext.Current);

            if (signId == null && signSize == null) return 0;
            if (signId >= 0)
            {
                signSize.SignId = signId ?? 0;
                HttpContext.Current.Session["__SessionSignSize"] = signSize;
                return (int)signId;
            }
            else
            {
                return signSize.SignId;
            }
        }
    }
}