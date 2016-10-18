using ImageProcessor.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.SessionState;
using Serilog;

namespace ImageProcessor.CP5200
{
    public class Cp5200External
    {
        private readonly ushort _screenWidth;
        private readonly ushort _screenHeight;
        private readonly ushort _displayTime;
        private readonly byte _colourMode;



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

        [DllImport("CP5200.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int CP5200_Program_SetWindowProperty(IntPtr hobj, int nWinNo, int nPropertyValue,
            int nPropertyId);


        [DllImport("CP5200.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int CP5200_Program_SaveToFile(IntPtr hObj,
            [MarshalAs(UnmanagedType.LPStr)] string pFileName);

        [DllImport("CP5200.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int CP5200_Program_Destroy(IntPtr hObj);
            
        [DllImport( "CP5200.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr CP5200_Playbill_Create(ushort width, ushort height, byte colour);

        [DllImport("CP5200.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ushort CP5200_Playbill_SetProperty(IntPtr hObj, ushort nPropertyValue, uint nPropertyId);


        [DllImport("CP5200.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int CP5200_Playbill_AddFile(IntPtr hObj, [MarshalAs(UnmanagedType.LPStr)] string pFilename);

        [DllImport("CP5200.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int CP5200_Playbill_SaveToFile(IntPtr hObj,
           [MarshalAs(UnmanagedType.LPStr)] string pFileName);




        public Cp5200External(ushort width, ushort height, ushort displayTime, byte colourMode)
        {
            _screenWidth = width;
            _screenHeight = height;
            _displayTime = displayTime;
            _colourMode = colourMode;
        }
    }
    public class PlayBillFiles
    {
        private IntPtr ProgramPointer; // { get; set; }
        private IntPtr PlaybillPointer; // { get; set; }
        private readonly ushort _screenWidth;
        private readonly ushort _screenHeight;
        private readonly ushort _displayTime;
        private readonly byte _colourMode;
        private readonly ILogger _logger;
        private int _playWindowNumber;
        public PlayBillFiles(ushort width, ushort height, ushort displayTime, byte colourMode)
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

                ProgramPointer = Cp5200External.CP5200_Program_Create(_screenWidth, _screenHeight, _colourMode);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Program create threw an error");
                return false;
            }
        }

        public void AddPlayWindow()
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
            return Cp5200External.CP5200_Program_SetProperty(ProgramPointer, propertyValue, propertyId);
        }

        public int Program_AddPlayWindow()
        {
            ushort xStart = 0;
            ushort yStart = 0;
            var result= Cp5200External.CP5200_Program_AddPlayWindow(ProgramPointer, xStart, yStart, _screenWidth, _screenHeight);


            return result;
        }
        public int Program_AddPicture(string path, int mode, int effect, int speed, int stayTime, int compress)
        {
            return Cp5200External.CP5200_Program_AddPicture(ProgramPointer, _playWindowNumber, path, mode, effect, speed, stayTime, compress);
        }
        public int Program_SaveFile(string filePathAndName)
        {
            try
            {
                System.IO.File.Delete(filePathAndName);
                return Cp5200External.CP5200_Program_SaveToFile(ProgramPointer, filePathAndName);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Program save threw error");
                throw;
            }
        }

        //Now move on to playbill files
        public bool playBill_Create()
        {
            try
            {

                PlaybillPointer = Cp5200External.CP5200_Playbill_Create(_screenWidth, _screenHeight, _colourMode);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "PlayBill create threw an error");
                return false;
            }
        }
        public int Playbill_SetProperty(ushort propertyValue, uint propertyId)
        {
            //1:Rotate 90 degrees
            return Cp5200External.CP5200_Playbill_SetProperty(PlaybillPointer, propertyValue, propertyId);
        }

        public int Playbill_AddFile(string path)
        {
            return Cp5200External.CP5200_Playbill_AddFile(PlaybillPointer,  path);         
        }


        public int Playbill_SaveToFile(string filePathAndName)
        {
            try
            {
                System.IO.File.Delete(filePathAndName);
                return Cp5200External.CP5200_Playbill_SaveToFile(PlaybillPointer, filePathAndName);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Program save threw error");
                throw;
            }
        }


    }
}

