using System;
using System.Collections.Generic;
using System.IO;
using ImageProcessor.CP5200;
using ImageProcessor.Enums;
using nightowlsign.data;
using nightowlsign.data.Models;
using nightowlsign.data.Models.Signs;
using System.Web;
using System.Linq;
using System.Net;

namespace ImageProcessor.Services

{
    public class CreateFilesToSend
    {
        private PlayBillFiles cp5200;
        private const string ImageDirectory = "~/playBillFiles/Images/";
        private const string ProgramFileDirectory = "~/playBillFiles/";
        public List<string> ProgramFiles { get; set; }
        public string PlaybillFileName { get; set; }

        private readonly List<SignDto> _signSizesForSchedule;
        private readonly List<ImageSelect> _imagesToSend;
        private readonly SignManager sm = new SignManager();

        public string DebugString { get; set; }

        public CreateFilesToSend(List<SignDto> signsForSchedule, List<ImageSelect> imagesToSend)
        {
            _signSizesForSchedule = signsForSchedule;
            _imagesToSend = imagesToSend;
        }

        public void PopulateSignList()
        {
            foreach (var signDto in _signSizesForSchedule)
            {
                sm.Find(signDto.Id);
            }
        }

        public void WriteImagesToDisk()
        {
            var counter = 1;
            foreach (var image in _imagesToSend)
            {
                var tempFileName = GenerateImageFileName(string.Format("{0:0000}0000", counter), image);
                counter += 1;
                image.Dispose();
            }
        }

        public void GeneratetheProgramFiles(string scheduleName)
        {
            var PlayItemNo = -1;
            var PeriodToShowImage = 0xA; //Seconds
            byte colourMode = 0x77;
            ProgramFiles = new List<string>();
            foreach (var signSize in _signSizesForSchedule)
            {
                ushort screenWidth = (ushort)(signSize.Width);
                ushort screenHeight = (ushort)(signSize.Height);
                cp5200 = new PlayBillFiles(screenWidth, screenHeight, PeriodToShowImage, colourMode);

                var counter = 1;
                foreach (
                    string fileName in Directory.GetFiles(HttpContext.Current.Server.MapPath(ImageDirectory), "*.jpg"))
                {
                    var programPointer = cp5200.Program_Create();
                    if (programPointer.ToInt32() > 0)
                    {
                        if (cp5200.AddPlayWindow(programPointer) >= 0)
                        {
                            PlayItemNo = cp5200.Program_AddPicture(programPointer, fileName,
                                (int)RenderMode.Stretch_to_fit_the_window, 0,
                                0, PeriodToShowImage, 0);

                            DebugString += string.Format("{0}Play Item Number: {1}, Temp File Name  {2}",
                                Environment.NewLine, PlayItemNo, fileName);

                            //  var programFileName = GenerateProgramFileName(string.Format("{0:0000}0000", counter));
                            // ProgramFiles.Add(programFileName);
                            if (
                                cp5200.Program_SaveFile(programPointer,
                                    GenerateProgramFileName(string.Format("{0:0000}0000", counter))) > 1)
                            {
                                cp5200.DestroyProgram(programPointer);
                            }
                            ;
                            //     cp5200.Playbill_AddFile(programFileName);
                        }
                        ;

                    }
                    counter += 1;
                }

            }
        }

        public void GeneratethePlayBillFile(string scheduleName)
        {
            var playBillPointer = cp5200.playBill_Create();

            if (playBillPointer.ToInt32() > 0)
            {
                if (playBillPointer.ToInt32() > 0)
                    cp5200.Playbill_SetProperty(playBillPointer, 0, 1);
                foreach (
                    string programFileName in
                    Directory.GetFiles(HttpContext.Current.Server.MapPath(ProgramFileDirectory), "*.lpb"))
                {
                    cp5200.Playbill_AddFile(playBillPointer, programFileName);
                }
                cp5200.Playbill_SaveToFile(playBillPointer, GeneratePlayBillFileName(scheduleName));
            }
        }

        private string GenerateImageFileName(string sCounter, ImageSelect image)
        {
            string tempFileName =
                HttpContext.Current.Server.MapPath(string.Concat("~/playBillFiles/images/", sCounter, ".jpg"));
            try
            {
                using (Stream stream = new FileStream(tempFileName, FileMode.Open))
                {
                    // File/Stream manipulating code here
                }
                System.IO.File.Delete(tempFileName);
                System.Threading.Thread.Sleep(1000);
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(image.ImageUrl, tempFileName);
                }
                return tempFileName;
            }
            catch (Exception ex)
            {
                return ex.InnerException.ToString();
                //check here why it failed and ask user to retry if the file is in use.
            }
        }

        private string GeneratePlayBillFileName(string scheduleName)
        {
            return PlaybillFileName = HttpContext.Current.Server.MapPath(string.Concat("~/playBillFiles/", strip(scheduleName), ".lpp"));
            //return HttpContext.Current.Server.MapPath(string.Concat("~/playBillFiles/", strip(scheduleName), ".lpp"));
        }

        private string GenerateProgramFileName(string sCounter)
        {
            var fileAndPath = HttpContext.Current.Server.MapPath(string.Concat("~/playBillFiles/", sCounter, ".lpb"));
            System.IO.File.Delete(fileAndPath);
            return fileAndPath;
        }

        private string strip(string scheduleName)
        {
            var invalidChars = " *./\\$";
            string invalidCharsRemoved = new string(scheduleName
              .Where(x => !invalidChars.Contains(x))
              .ToArray());
            return invalidCharsRemoved;
        }


    }
}



