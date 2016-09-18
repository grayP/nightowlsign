using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Configuration;
using imageprocessor.CP5200;
using imageprocessor.Enums;

namespace imageprocessor
{
    public class Program
    {


        //public static extern IntPtr CP5200_Program_Create(ushort width, ushort height, byte colour);
        //[DllImport("CP5200.dll", CallingConvention = CallingConvention.StdCall)]
        //public static extern ushort CP5200_Program_SetProperty(IntPtr hObj, ushort nPropertyValue, uint nPropertyId);
        //[DllImport("CP5200.dll", CallingConvention = CallingConvention.StdCall)]
        //public static extern int CP5200_Program_AddPicture(IntPtr hObj, int nWinNo,
        //  [MarshalAs(UnmanagedType.LPStr)] string pPictFile, int nMode, int nEffect, int nSpeed, int nStay,
        //  int nCompress);
       


        static void Main(string[] args)
        {
            int PlayItemNo = -1;
          

            ushort screenWidth = 206;
            ushort screenHeight = 66;
            ushort PeriodToShowImage = 6; //Seconds
            byte colourMode = 0x77;


            CP5200External cp5200 = new CP5200External(screenWidth, screenHeight,PeriodToShowImage, colourMode);



            if (cp5200.Program_Create())
            {
                cp5200.SetPlayWindowNumber();
            }



            PlayItemNo = cp5200.Program_AddPicture("D:\\nightowl\\nightowlsigns\\nightowlsigns\\Content\\images\\night_owl.gif",  (int)RenderMode.Zoom_to_fit_the_window, 0, 0, 6, 1);
  
            Console.WriteLine(PlayItemNo);

            Console.Read();


        }
    }
}



