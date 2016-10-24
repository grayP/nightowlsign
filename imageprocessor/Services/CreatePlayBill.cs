using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Configuration;
using ImageProcessor.CP5200;
using ImageProcessor.Enums;
using nightowlsign.data;
using nightowlsign.data.Models;
using nightowlsign.data.Models.Signs;
using System.Web;

namespace ImageProcessor.Services

{
    public class CreatePlayBill
    {
        private readonly List<SignDto> _SignSizesForSchedule;
        private readonly List<ImageSelect> _imagesToSend;
        private readonly SignManager sm = new SignManager();

        //public static extern IntPtr CP5200_Program_Create(ushort width, ushort height, byte colour);
        //[DllImport("CP5200.dll", CallingConvention = CallingConvention.StdCall)]
        //public static extern ushort CP5200_Program_SetProperty(IntPtr hObj, ushort nPropertyValue, uint nPropertyId);
        //[DllImport("CP5200.dll", CallingConvention = CallingConvention.StdCall)]
        //public static extern int CP5200_Program_AddPicture(IntPtr hObj, int nWinNo,
        //  [MarshalAs(UnmanagedType.LPStr)] string pPictFile, int nMode, int nEffect, int nSpeed, int nStay,
        //  int nCompress);

        public CreatePlayBill(List<SignDto> signsForSchedule, List<ImageSelect> imagesToSend)
        {
            _SignSizesForSchedule = signsForSchedule;
            _imagesToSend = imagesToSend;
        }

        public void PopulateSignList()
        {
            foreach (var signDto in _SignSizesForSchedule)
            {
               sm.Find(signDto.Id);

            }
        }
        public void GeneratethePlayBillFile(string scheduleName)
        {
            int PlayItemNo = -1;
          
            ushort PeriodToShowImage = 6; //Seconds
            byte colourMode = 0x77;
            foreach (var signSize in _SignSizesForSchedule)
            {
                ushort screenWidth = (ushort)(signSize.Width);
                ushort screenHeight = (ushort)(signSize.Height);

                Cp5200External cp5200 = new Cp5200External(screenWidth, screenHeight, PeriodToShowImage, colourMode);

                if (cp5200.Program_Create())
                {
                    cp5200.SetPlayWindowNumber();
                }

                foreach (var image in _imagesToSend)
                {
                  PlayItemNo = cp5200.Program_AddPicture(image.ImageUrl, (int)RenderMode.Stretch_to_fit_the_window,1, 1, PeriodToShowImage, 1);
                }
                var FileName =
                    HttpContext.Current.Server.MapPath(string.Concat("/playBillFiles/", strip(scheduleName), ".lpl"));
                cp5200.Program_SaveFile(FileName);

            }

        }

        private object strip(string scheduleName)
        {
            scheduleName = scheduleName.Replace(" ", "");
            scheduleName = scheduleName.Replace("*", "");
            scheduleName = scheduleName.Replace(".", "");
            scheduleName = scheduleName.Replace("/", "");
            scheduleName = scheduleName.Replace("$", "");
            return scheduleName;

        }
    }
}



