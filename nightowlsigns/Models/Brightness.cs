using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using nightowlsign.data;

namespace nightowlsign.Models
{
    public class Brightness
    {
        public int SignId { get; set; }
        private int[] BrightnessLevel { get; set; }
        public List<SignBrightness> BrightnessList { get; set; }
    }


}