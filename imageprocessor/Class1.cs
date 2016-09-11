using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace imageprocessor
{

   

  





    public class Program
    {
 
        [DllImport("D:\\nightowl\\cplus\\WriteToSign\\Debug\\CP5200.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int CP5200_Program_SetProperty(object hObj, int nPropertyValue, uint nPropertyId);
        [DllImport("D:\\nightowl\\cplus\\WriteToSign\\Debug\\CP5200.dll", CallingConvention = CallingConvention.StdCall) ]
        public static extern IntPtr CP5200_Program_Create(ushort width, ushort height, byte colour);
        [DllImport("D:\\nightowl\\cplus\\WriteToSign\\Debug\\CP5200.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int CP5200_Program_AddPicture(object hObj, int nWinNo,
            [MarshalAs(UnmanagedType.LPStr)] string pPictFile, int nMode, int nEffect, int nSpeed, int nStay,
            int nCompress);


        static void Main(string[] args)
        {

            IntPtr Object =CP5200_Program_Create(206, 66, 0x01);
            var IntReturn = CP5200_Program_SetProperty(Object, 2, (uint)10);
            var Intreturn2 = CP5200_Program_AddPicture(Object, 0,
                "D:\\nightowl\\nightowlsigns\\nightowlsigns\\Content\\images\\graphic.png", 2, 1, 0, 10, 1);

            Console.WriteLine("hello");
            Console.Read();


        }






   

    }
}



