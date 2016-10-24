using ImageProcessor.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
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

        private const string DllPath = "CP5200.dll";

        [DllImport(DllPath, CharSet = CharSet.Auto)]
        public static extern IntPtr CP5200_Program_Create(int width, int height, byte color);

        [DllImport(DllPath, CharSet = CharSet.Auto)]
        public static extern ushort CP5200_Program_SetProperty(IntPtr hObj, int nPropertyValue, uint nPropertyId);

        [DllImport(DllPath, CallingConvention = CallingConvention.StdCall)]
        public static extern int CP5200_Program_AddPicture(IntPtr hObj, int nWinNo,
            [MarshalAs(UnmanagedType.LPStr)] string pPictFile, int nMode, int nEffect, int nSpeed, int nStay,
            int nCompress);

        [DllImport(DllPath, CallingConvention = CallingConvention.StdCall)]
        public static extern int CP5200_Program_AddPlayWindow(IntPtr hobj, int x, int y, int cx, int cy);

        [DllImport(DllPath, CallingConvention = CallingConvention.StdCall)]
        public static extern int CP5200_Program_SetWindowProperty(IntPtr hobj, int nWinNo, int nPropertyValue,
            int nPropertyId);


        [DllImport(DllPath, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int CP5200_Program_SaveToFile(IntPtr hObj,
            [MarshalAs(UnmanagedType.LPStr)] string pFileName);

        [DllImport(DllPath, CallingConvention = CallingConvention.StdCall)]
        public static extern int CP5200_Program_Destroy(IntPtr hObj);

        [DllImport(DllPath, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr CP5200_Playbill_Create(int width, int height, byte colour);

        [DllImport(DllPath, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort CP5200_Playbill_SetProperty(IntPtr hObj, ushort nPropertyValue, uint nPropertyId);


      //  [DllImport(DllPath, CallingConvention = CallingConvention.StdCall)]
       // public static extern int CP5200_Playbill_AddFile(IntPtr hObj, [MarshalAs(UnmanagedType.LPStr)] string pFilename);

        [DllImport(DllPath, CharSet = CharSet.Auto)]
        public static extern int CP5200_Playbill_AddFile(IntPtr hObj, IntPtr pFilename);


        [DllImport(DllPath, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int CP5200_Playbill_SaveToFile(IntPtr hObj,
           [MarshalAs(UnmanagedType.LPStr)] string pFileName);


        //Network 
        [DllImport(DllPath, CharSet = CharSet.Auto)]
        public static extern int CP5200_Net_Init(uint dwIP, int nIPPort, uint dwIDCode, int nTimeOut);


        [DllImport(DllPath, CharSet = CharSet.Auto)]
        public static extern int CP5200_Net_UploadFile(int nCardID, IntPtr pSourceFilename, IntPtr pTargetFilename);

        [DllImport(DllPath, CharSet = CharSet.Auto)]
        public static extern int CP5200_Net_RestartApp(byte nCardID);


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
        private readonly int _screenWidth;
        private readonly int _screenHeight;
        private readonly int _displayTime;
        private readonly byte _colourMode;
        private readonly ILogger _logger;
        private int _playWindowNumber;
        private int TimeOut = 3600;
        public PlayBillFiles(int width, int height, int displayTime, byte colourMode)
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
                ProgramPointer = Cp5200External.CP5200_Program_Create(_screenWidth, _screenHeight, 0x77);
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

        public int Program_SetProperty(int propertyValue, uint propertyId)
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
            var result = Cp5200External.CP5200_Program_AddPlayWindow(ProgramPointer, xStart, yStart, _screenWidth, _screenHeight);


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
             //   PlaybillPointer = Cp5200External.CP5200_Playbill_Create(_screenWidth, _screenHeight, _colourMode);
                PlaybillPointer = Cp5200External.CP5200_Playbill_Create(Convert.ToInt32(_screenWidth), Convert.ToInt32(_screenHeight), 0x77);
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
            var intRet= Cp5200External.CP5200_Playbill_AddFile(PlaybillPointer, GetProgramFileName(path));
            return intRet;
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

        internal void DestroyProgram()
        {
            Cp5200External.CP5200_Program_Destroy(ProgramPointer);
        }

        private void InitComm(string ipAddress, string idCode, string port)
        {
            try
            {
                uint dwIPAddr = GetIP(ipAddress);
                uint dwIDCode = GetIP(idCode);
                int nIPPort = Convert.ToInt32(port);
                if (dwIPAddr != 0 && dwIDCode != 0)
                    Cp5200External.CP5200_Net_Init(dwIPAddr, nIPPort, dwIDCode, TimeOut);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void SendFiletoSign(string ProgramFileName, string PlayBillFilename)
        {
            try
            {
            //    InitComm("192.168.1.222","255.255.255.255","5200");

                int uploadCount = 0;
                if (0 ==
                    Cp5200External.CP5200_Net_UploadFile(Convert.ToByte(1), GetProgramFileName(ProgramFileName),
                        GetProgramFileName(ProgramFileName)))
                    uploadCount++;

                if (0 ==
                    Cp5200External.CP5200_Net_UploadFile(Convert.ToByte(1), GetPlaybillFileName(PlayBillFilename),
                        GetPlaybillFileName(PlayBillFilename)))
                    uploadCount++;

                if (uploadCount > 0)
                    Cp5200External.CP5200_Net_RestartApp(Convert.ToByte(1));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
        private uint GetIP(string strIp)
        {
            System.Net.IPAddress ipaddress = System.Net.IPAddress.Parse(strIp);
            uint lIp = (uint)ipaddress.Address;
            lIp = ((lIp & 0xFF000000) >> 24) + ((lIp & 0x00FF0000) >> 8) + ((lIp & 0x0000FF00) << 8) + ((lIp & 0x000000FF) << 24);
            return (lIp);
        }
        IntPtr GetProgramFileName(string fileName)
        {
            return Marshal.StringToHGlobalAnsi(fileName);
        }

        IntPtr GetPlaybillFileName(string fileName)
        {
            return Marshal.StringToHGlobalAnsi(fileName);
        }

        internal int Program_SaveFile(object programFileName)
        {
            throw new NotImplementedException();
        }
    }
}

