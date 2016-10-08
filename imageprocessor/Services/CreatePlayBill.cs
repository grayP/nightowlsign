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

namespace ImageProcessor.Services

{
    public class CreatePlayBill
    {
        private readonly List<SignDto> _SignSizesForSchedule;
        private readonly List<ImageSelect> _imagesToSend;

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
        public void GeneratethePlayBillFile()
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
                  PlayItemNo = cp5200.Program_AddPicture(image.ImageUrl, (int)RenderMode.Zoom_to_fit_the_window, 0, 0, PeriodToShowImage, 1);
                   // PlayItemNo = cp5200.Program_AddPicture("D:\\nightowl\\nightowlsigns\\nightowlsigns\\Content\\images\\night_owl.gif", (int)RenderMode.Zoom_to_fit_the_window, 0, 0, PeriodToShowImage, 1);
                }
                Console.WriteLine(PlayItemNo);

                Console.Read();
            }

        }
    }
}



