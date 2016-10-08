using ImageProcessor.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessor.CP5200
{
    public class Cp5200External
    {
        public IntPtr Pointer { get; set; }
        private readonly ushort _screenWidth;
        private readonly ushort _screenHeight;
        private readonly ushort _displayTime;
        private readonly byte _colourMode;

        private int _playWindowNumber;

        [DllImport("CP5200.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr CP5200_Program_Create(ushort width, ushort height, byte colour);
        [DllImport("CP5200.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ushort CP5200_Program_SetProperty(IntPtr hObj, ushort nPropertyValue, uint nPropertyId);
        [DllImport("CP5200.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int CP5200_Program_AddPicture(IntPtr hObj, int nWinNo,
          [MarshalAs(UnmanagedType.LPStr)] string pPictFile, int nMode, int nEffect, int nSpeed, int nStay,
          int nCompress);

        [DllImport("CP5200.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int CP5200_Program_AddPlayWindow(IntPtr hobj, ushort x, ushort y, ushort cx, ushort cy);

        public Cp5200External(ushort width, ushort height, ushort displayTime, byte colourMode)
        {
            _screenWidth = width;
            _screenHeight = height;
            _displayTime = displayTime;
            _colourMode = colourMode;
        }

        public bool Program_Create()
        {
            try
            {
                Pointer = CP5200_Program_Create(_screenWidth, _screenHeight, _colourMode);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException.ToString());
                return false;
            }

        }

        public void SetPlayWindowNumber()
        {
            if (Program_SetProperty(_displayTime, (uint)Set_Program_Property.ProgramPlayTime) > 0)
            {
                _playWindowNumber = Program_AddPlayWindow();
            }
        }

        public int Program_SetProperty(ushort propertyValue, uint propertyId)
        {
            //1: program repetition play times
            //2: program play time
            //3: Code conversion
            return CP5200_Program_SetProperty(Pointer, propertyValue, propertyId);
        }

        public int Program_AddPicture(string path, int mode, int effect, int speed, int stayTime, int compress)
        {
            return CP5200_Program_AddPicture(Pointer, _playWindowNumber, path, mode, effect, speed, stayTime, compress);
        }

        public int Program_AddPlayWindow()
        {
            return CP5200_Program_AddPlayWindow(Pointer, 0, 0, _screenWidth, _screenHeight);
        }
    }
}

