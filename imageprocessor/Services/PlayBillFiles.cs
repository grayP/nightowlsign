using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ImageProcessor.CP5200;
using ImageProcessor.Enums;
using Serilog;

namespace ImageProcessor.Services
{
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

        public int AddPlayWindow()
        {
            if (Program_SetProperty(_displayTime, (uint)Set_Program_Property.ProgramPlayTime) > 0)
            {
                _playWindowNumber = Program_AddPlayWindow();
                return _playWindowNumber;

            }
            return -1;
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
            return Cp5200External.CP5200_Playbill_AddFile(PlaybillPointer, Marshal.StringToHGlobalAnsi(path));

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
    }
}
